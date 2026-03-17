import { Page, Route } from '@playwright/test';

type RefCompany = {
    id: string;
    code: string;
    name: string;
    isActive: boolean;
};

type RefRegion = {
    id: string;
    companyId?: string;
    code: string;
    name: string;
    isActive: boolean;
};

type RefSeverity = {
    id: string;
    code: string;
    name: string;
    sortOrder: number;
};

type RefReferenceType = {
    id: string;
    code: string;
    name: string;
    appliesTo: string;
};

type RefOption = {
    id: string;
    referenceTypeCode: string;
    code: string;
    name: string;
    isActive?: boolean;
};

type WorkflowState = {
    id: string;
    code: string;
    name: string;
    sortOrder: number;
};

type IncidentReport = {
    id: string;
    incidentNumber: string;
    incidentDate: string;
    companyId?: string;
    companyName?: string;
    regionId?: string;
    regionName?: string;
    jobNumber?: string;
    clientCode?: string;
    plantCode?: string;
    incidentClass?: string;
    severityActualCode?: string;
    severityPotentialCode?: string;
    workDescription?: string;
    incidentSummary?: string;
    status?: string;
    referenceIds?: string[];
    employeesInvolved?: unknown[];
    actions?: unknown[];
};

export type MockApiState = {
    companies: RefCompany[];
    regions: RefRegion[];
    severities: RefSeverity[];
    referenceTypes: RefReferenceType[];
    lookupItems: RefOption[];
    workflowStates: WorkflowState[];
    incidents: IncidentReport[];
};

const JSON_HEADERS = {
    'access-control-allow-origin': '*',
    'content-type': 'application/json',
};

function replyJson(route: Route, status: number, payload: unknown) {
    return route.fulfill({
        status,
        headers: JSON_HEADERS,
        body: JSON.stringify(payload),
    });
}

function replyEmpty(route: Route, status = 204) {
    return route.fulfill({
        status,
        headers: {
            'access-control-allow-origin': '*',
        },
    });
}

function replyNotFound(route: Route, path: string) {
    return replyJson(route, 404, {
        title: 'Not Found',
        status: 404,
        detail: `No mock route for ${path}`,
    });
}

function getQuery(url: URL, key: string): string | null {
    const lower = key.toLowerCase();
    for (const [name, value] of url.searchParams.entries()) {
        if (name.toLowerCase() === lower) {
            return value;
        }
    }
    return null;
}

function parseBody(requestBody: string | null): Record<string, unknown> {
    if (!requestBody) {
        return {};
    }

    try {
        return JSON.parse(requestBody) as Record<string, unknown>;
    } catch {
        return {};
    }
}

function buildIncidentOptions(state: MockApiState): Record<string, RefOption[]> {
    const groups: Record<string, RefOption[]> = {};
    for (const item of state.lookupItems) {
        const typeCode = item.referenceTypeCode;
        if (!groups[typeCode]) {
            groups[typeCode] = [];
        }
        groups[typeCode].push(item);
    }
    return groups;
}

function incidentToListItem(incident: IncidentReport): IncidentReport {
    return {
        id: incident.id,
        incidentNumber: incident.incidentNumber,
        incidentDate: incident.incidentDate,
        companyId: incident.companyId,
        companyName: incident.companyName,
        regionId: incident.regionId,
        regionName: incident.regionName,
        jobNumber: incident.jobNumber,
        severityActualCode: incident.severityActualCode,
        status: incident.status ?? 'FIRSTREPORT',
    };
}

function seedState(): MockApiState {
    return {
        companies: [
            { id: 'co-1', code: 'CCI', name: 'Central Construction Inc', isActive: true },
            { id: 'co-2', code: 'TET', name: 'Texas Energy Transport', isActive: true },
        ],
        regions: [
            { id: 're-1', companyId: 'co-1', code: 'SW', name: 'Southwest', isActive: true },
            { id: 're-2', companyId: 'co-2', code: 'SE', name: 'Southeast', isActive: true },
        ],
        severities: [
            { id: 'sev-1', code: 'Minor', name: 'Minor', sortOrder: 1 },
            { id: 'sev-2', code: 'Serious', name: 'Serious', sortOrder: 2 },
            { id: 'sev-3', code: 'Major', name: 'Major', sortOrder: 3 },
        ],
        referenceTypes: [
            { id: 'rt-1', code: 'environmental', name: 'Environmental', appliesTo: 'Incident' },
            { id: 'rt-2', code: 'injury_type', name: 'Injury Type', appliesTo: 'Incident' },
            { id: 'rt-3', code: 'investigation_required', name: 'Investigation Required', appliesTo: 'Incident' },
        ],
        lookupItems: [
            { id: 'li-1', referenceTypeCode: 'environmental', code: 'release', name: 'Release', isActive: true },
            { id: 'li-2', referenceTypeCode: 'environmental', code: 'spill', name: 'Spill', isActive: true },
            { id: 'li-3', referenceTypeCode: 'injury_type', code: 'struck-by', name: 'Struck By', isActive: true },
            { id: 'li-4', referenceTypeCode: 'investigation_required', code: 'formal', name: 'Formal Investigation', isActive: true },
        ],
        workflowStates: [
            { id: 'wf-1', code: 'FIRSTREPORT', name: 'First Report', sortOrder: 1 },
            { id: 'wf-2', code: 'SUBMIT', name: 'Submitted', sortOrder: 2 },
            { id: 'wf-3', code: 'INVESTIGATION', name: 'Investigation', sortOrder: 3 },
            { id: 'wf-4', code: 'CLOSED', name: 'Closed', sortOrder: 4 },
        ],
        incidents: [
            {
                id: 'inc-1',
                incidentNumber: 'CCI-2026-0001',
                incidentDate: '2026-01-15T14:22:00.000Z',
                companyId: 'co-1',
                companyName: 'Central Construction Inc',
                regionId: 're-1',
                regionName: 'Southwest',
                jobNumber: 'J-1028',
                clientCode: 'Acme Refinery',
                plantCode: 'Plant 7',
                incidentClass: 'Actual',
                severityActualCode: 'Minor',
                severityPotentialCode: 'Serious',
                workDescription: 'Crew was replacing a pressure valve.',
                incidentSummary: 'Minor hand scrape while handling fittings.',
                status: 'FIRSTREPORT',
                referenceIds: ['li-1'],
                employeesInvolved: [],
                actions: [],
            },
        ],
    };
}

export async function installMockApi(page: Page): Promise<MockApiState> {
    const state = seedState();
    let numericId = 10;

    const nextId = (prefix: string) => {
        numericId += 1;
        return `${prefix}-${numericId}`;
    };

    await page.route('**/v1/ReferenceData/**', async (route) => {
        const request = route.request();
        const method = request.method().toUpperCase();
        const url = new URL(request.url());
        const pathLower = url.pathname.toLowerCase();
        const body = parseBody(request.postData());

        if (method === 'GET' && pathLower.endsWith('/v1/referencedata/companies')) {
            return replyJson(route, 200, state.companies);
        }

        if (method === 'POST' && pathLower.endsWith('/v1/referencedata/companies')) {
            const company: RefCompany = {
                id: nextId('co'),
                code: String(body.code ?? '').trim(),
                name: String(body.name ?? '').trim(),
                isActive: body.isActive !== false,
            };
            state.companies.push(company);
            return replyJson(route, 200, company);
        }

        const companyIdMatch = pathLower.match(/\/v1\/referencedata\/companies\/([^/?#]+)/);
        if (companyIdMatch && method === 'PUT') {
            const id = decodeURIComponent(companyIdMatch[1]);
            const index = state.companies.findIndex((x) => x.id === id);
            if (index === -1) {
                return replyNotFound(route, url.pathname);
            }
            const updated = {
                ...state.companies[index],
                code: String(body.code ?? state.companies[index].code),
                name: String(body.name ?? state.companies[index].name),
                isActive: body.isActive === undefined ? state.companies[index].isActive : Boolean(body.isActive),
            };
            state.companies[index] = updated;
            return replyJson(route, 200, updated);
        }

        if (companyIdMatch && method === 'DELETE') {
            const id = decodeURIComponent(companyIdMatch[1]);
            state.companies = state.companies.filter((x) => x.id !== id);
            return replyEmpty(route);
        }

        if (method === 'GET' && pathLower.endsWith('/v1/referencedata/regions')) {
            const companyId = getQuery(url, 'companyId');
            const regions = companyId
                ? state.regions.filter((x) => x.companyId === companyId)
                : state.regions;
            return replyJson(route, 200, regions);
        }

        if (method === 'POST' && pathLower.endsWith('/v1/referencedata/regions')) {
            const region: RefRegion = {
                id: nextId('re'),
                companyId: (body.companyId as string | undefined) ?? undefined,
                code: String(body.code ?? '').trim(),
                name: String(body.name ?? '').trim(),
                isActive: body.isActive !== false,
            };
            state.regions.push(region);
            return replyJson(route, 200, region);
        }

        const regionIdMatch = pathLower.match(/\/v1\/referencedata\/regions\/([^/?#]+)/);
        if (regionIdMatch && method === 'PUT') {
            const id = decodeURIComponent(regionIdMatch[1]);
            const index = state.regions.findIndex((x) => x.id === id);
            if (index === -1) {
                return replyNotFound(route, url.pathname);
            }
            const updated = {
                ...state.regions[index],
                companyId: (body.companyId as string | undefined) ?? undefined,
                code: String(body.code ?? state.regions[index].code),
                name: String(body.name ?? state.regions[index].name),
                isActive: body.isActive === undefined ? state.regions[index].isActive : Boolean(body.isActive),
            };
            state.regions[index] = updated;
            return replyJson(route, 200, updated);
        }

        if (regionIdMatch && method === 'DELETE') {
            const id = decodeURIComponent(regionIdMatch[1]);
            state.regions = state.regions.filter((x) => x.id !== id);
            return replyEmpty(route);
        }

        if (method === 'GET' && pathLower.endsWith('/v1/referencedata/reference-types')) {
            return replyJson(route, 200, state.referenceTypes);
        }

        if (method === 'GET' && pathLower.endsWith('/v1/referencedata/incident-options')) {
            return replyJson(route, 200, buildIncidentOptions(state));
        }

        if (method === 'GET' && pathLower.endsWith('/v1/referencedata/severities')) {
            return replyJson(route, 200, state.severities);
        }

        if (method === 'GET' && pathLower.endsWith('/v1/referencedata/workflow-states')) {
            return replyJson(route, 200, state.workflowStates);
        }

        if (method === 'POST' && pathLower.endsWith('/v1/referencedata/lookup-items')) {
            const type = state.referenceTypes.find((x) => x.id === body.referenceTypeId);
            const item: RefOption = {
                id: nextId('li'),
                referenceTypeCode: type?.code ?? 'unknown',
                code: String(body.code ?? '').trim(),
                name: String(body.name ?? '').trim(),
                isActive: body.isActive !== false,
            };
            state.lookupItems.push(item);
            return replyJson(route, 200, item);
        }

        const lookupIdMatch = pathLower.match(/\/v1\/referencedata\/lookup-items\/([^/?#]+)/);
        if (lookupIdMatch && method === 'PUT') {
            const id = decodeURIComponent(lookupIdMatch[1]);
            const index = state.lookupItems.findIndex((x) => x.id === id);
            if (index === -1) {
                return replyNotFound(route, url.pathname);
            }
            const type = state.referenceTypes.find((x) => x.id === body.referenceTypeId);
            const updated = {
                ...state.lookupItems[index],
                referenceTypeCode: type?.code ?? state.lookupItems[index].referenceTypeCode,
                code: String(body.code ?? state.lookupItems[index].code),
                name: String(body.name ?? state.lookupItems[index].name),
                isActive: body.isActive === undefined ? state.lookupItems[index].isActive : Boolean(body.isActive),
            };
            state.lookupItems[index] = updated;
            return replyJson(route, 200, updated);
        }

        if (lookupIdMatch && method === 'DELETE') {
            const id = decodeURIComponent(lookupIdMatch[1]);
            state.lookupItems = state.lookupItems.filter((x) => x.id !== id);
            return replyEmpty(route);
        }

        return replyNotFound(route, url.pathname);
    });

    await page.route('**/v1/IncidentReport**', async (route) => {
        const request = route.request();
        const method = request.method().toUpperCase();
        const url = new URL(request.url());
        const pathLower = url.pathname.toLowerCase();
        const body = parseBody(request.postData());

        if (method === 'GET' && pathLower.endsWith('/v1/incidentreport')) {
            const search = getQuery(url, 'searchTerm')?.toLowerCase() ?? '';
            const companyId = getQuery(url, 'companyId');
            const status = getQuery(url, 'status');

            const list = state.incidents
                .filter((incident) => {
                    const matchesSearch = !search
                        || `${incident.incidentNumber} ${incident.companyName} ${incident.jobNumber} ${incident.incidentSummary}`
                            .toLowerCase()
                            .includes(search);
                    const matchesCompany = !companyId || incident.companyId === companyId;
                    const matchesStatus = !status || incident.status === status;
                    return matchesSearch && matchesCompany && matchesStatus;
                })
                .map(incidentToListItem);

            return replyJson(route, 200, list);
        }

        if (method === 'POST' && pathLower.endsWith('/v1/incidentreport')) {
            const companyId = (body.companyId as string | undefined) ?? state.companies[0]?.id;
            const company = state.companies.find((x) => x.id === companyId);
            const region = state.regions.find((x) => x.id === body.regionId) ?? state.regions[0];
            const companyCode = company?.code ?? 'INC';
            const year = new Date().getFullYear();
            const running = String(state.incidents.length + 1).padStart(4, '0');

            const created: IncidentReport = {
                id: nextId('inc'),
                incidentNumber: `${companyCode}-${year}-${running}`,
                incidentDate: String(body.incidentDate ?? new Date().toISOString()),
                companyId,
                companyName: company?.name ?? '',
                regionId: (body.regionId as string | undefined) ?? region?.id,
                regionName: region?.name,
                jobNumber: body.jobNumber as string | undefined,
                clientCode: body.clientCode as string | undefined,
                plantCode: body.plantCode as string | undefined,
                incidentClass: (body.incidentClass as string | undefined) ?? 'Actual',
                severityActualCode: body.severityActualCode as string | undefined,
                severityPotentialCode: body.severityPotentialCode as string | undefined,
                workDescription: body.workDescription as string | undefined,
                incidentSummary: body.incidentSummary as string | undefined,
                status: (body.status as string | undefined) ?? 'FIRSTREPORT',
                referenceIds: (body.referenceIds as string[] | undefined) ?? [],
                employeesInvolved: (body.employeesInvolved as unknown[] | undefined) ?? [],
                actions: (body.actions as unknown[] | undefined) ?? [],
            };

            state.incidents.unshift(created);
            return replyJson(route, 201, created);
        }

        const incidentMatch = pathLower.match(/\/v1\/incidentreport\/([^/?#]+)/);
        if (!incidentMatch) {
            return replyNotFound(route, url.pathname);
        }

        const incidentId = decodeURIComponent(incidentMatch[1]);
        const incidentIndex = state.incidents.findIndex((x) => x.id === incidentId);

        if (incidentIndex === -1) {
            return replyNotFound(route, url.pathname);
        }

        if (method === 'GET') {
            return replyJson(route, 200, state.incidents[incidentIndex]);
        }

        if (method === 'PUT') {
            const existing = state.incidents[incidentIndex];
            const updated: IncidentReport = {
                ...existing,
                ...body,
                id: existing.id,
                incidentNumber: existing.incidentNumber,
                companyName: state.companies.find((x) => x.id === (body.companyId ?? existing.companyId))?.name ?? existing.companyName,
                regionName: state.regions.find((x) => x.id === (body.regionId ?? existing.regionId))?.name ?? existing.regionName,
            };
            state.incidents[incidentIndex] = updated;
            return replyJson(route, 202, updated);
        }

        if (method === 'DELETE') {
            state.incidents = state.incidents.filter((x) => x.id !== incidentId);
            return replyEmpty(route);
        }

        return replyNotFound(route, url.pathname);
    });

    return state;
}



