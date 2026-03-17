import { APIRequestContext } from '@playwright/test';

export const API_BASE_URL = process.env.PW_API_BASE_URL ?? 'https://localhost:7207';
export const KEEP_TEST_DATA = process.env.PW_KEEP_TEST_DATA === 'true';

type IncidentListItem = {
    id?: string;
};

export async function getJsonOrFail<T>(request: APIRequestContext, url: string): Promise<T> {
    let response;

    try {
        response = await request.get(url, { failOnStatusCode: false });
    } catch (error) {
        throw new Error(`Could not reach ${url}. Is API running on ${API_BASE_URL}? ${String(error)}`);
    }

    if (!response.ok()) {
        const body = await response.text();
        throw new Error(`Request failed: GET ${url} -> ${response.status()} ${response.statusText()}\n${body}`);
    }

    return (await response.json()) as T;
}

export async function deleteIncidentById(request: APIRequestContext, incidentId: string): Promise<void> {
    const response = await request.delete(`${API_BASE_URL}/v1/IncidentReport/${incidentId}`, {
        failOnStatusCode: false,
    });

    if (response.status() !== 202 && response.status() !== 204 && response.status() !== 404) {
        const body = await response.text();
        throw new Error(
            `Failed to delete incident ${incidentId}: ${response.status()} ${response.statusText()}\n${body}`,
        );
    }
}

export async function cleanupIncidentsByJobTag(
    request: APIRequestContext,
    jobTag: string,
): Promise<void> {
    const query = encodeURIComponent(jobTag);
    const incidents = await getJsonOrFail<IncidentListItem[]>(
        request,
        `${API_BASE_URL}/v1/IncidentReport?searchTerm=${query}`,
    );

    for (const incident of incidents) {
        if (incident.id) {
            await deleteIncidentById(request, incident.id);
        }
    }
}