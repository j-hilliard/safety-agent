import { test, expect } from '@playwright/test';
import {
    expectNoRuntimeErrors,
    gotoAndStabilize,
    selectFirstPrimeDropdownOption,
    startRuntimeErrorTracking,
} from './utils/ui';
import {
    API_BASE_URL,
    cleanupIncidentsByJobTag,
    deleteIncidentById,
    getJsonOrFail,
    KEEP_TEST_DATA,
} from './utils/liveApi';

type RefCompanyDto = {
    id?: string;
};

type RefRegionDto = {
    id?: string;
};

test.describe('Live Incident Workflow (DB-backed)', () => {
    test.describe.configure({ timeout: 120_000 });

    test('double-clicking an incident row opens the incident detail view', async ({ page, request }) => {
        const runtimeErrors = startRuntimeErrorTracking(page);

        await getJsonOrFail<RefCompanyDto[]>(request, `${API_BASE_URL}/v1/ReferenceData/companies`);
        await getJsonOrFail<RefRegionDto[]>(request, `${API_BASE_URL}/v1/ReferenceData/regions`);

        await gotoAndStabilize(page, '/incident-management/incidents');
        const firstRow = page.locator('.p-datatable-tbody > tr').first();
        await expect(firstRow).toBeVisible();

        const incidentNumber = (await firstRow.locator('td').first().textContent())?.trim() ?? '';
        await firstRow.dblclick();
        await expect(page).toHaveURL(/\/incident-management\/incidents\/[0-9a-fA-F-]{36}$/);

        if (incidentNumber.length > 0) {
            await expect(page.getByText(incidentNumber, { exact: false }).first()).toBeVisible();
        }

        expectNoRuntimeErrors(runtimeErrors);
    });

    test('can select actual+potential severity, save draft, and cleanup created data', async ({ page, request }) => {
        const runtimeErrors = startRuntimeErrorTracking(page);
        const jobTag = `PW-LIVE-${Date.now()}`;
        let createdId: string | undefined;

        try {
            await cleanupIncidentsByJobTag(request, jobTag);

            await getJsonOrFail<RefCompanyDto[]>(request, `${API_BASE_URL}/v1/ReferenceData/companies`);
            await getJsonOrFail<RefRegionDto[]>(request, `${API_BASE_URL}/v1/ReferenceData/regions`);

            await gotoAndStabilize(page, '/incident-management/incidents/new');
            await expect(page.getByRole('heading', { name: 'New Incident Report' })).toBeVisible();

            await selectFirstPrimeDropdownOption(page, page.locator('.p-dropdown').first());

            const regionDropdown = page.locator('.p-dropdown').nth(1);
            await expect(regionDropdown).not.toHaveClass(/p-disabled/);
            await selectFirstPrimeDropdownOption(page, regionDropdown);

            const actualSeverityInput = page.locator('input[id^="sa-"]').nth(1);
            const potentialSeverityInput = page.locator('input[id^="sp-"]').nth(2);

            await page.locator('.p-radiobutton:has(input[id^="sa-"])').nth(1).click();
            await page.locator('.p-radiobutton:has(input[id^="sp-"])').nth(2).click();

            await expect(actualSeverityInput).toBeChecked();
            await expect(potentialSeverityInput).toBeChecked();

            await page.locator('xpath=//label[contains(normalize-space(.), "Job Number")]/following::input[1]').fill(jobTag);
            await page.getByPlaceholder('Describe the work being performed at the time of the incident').fill('Playwright live save validation.');
            await page.getByPlaceholder('Provide a detailed description of the incident').fill('Playwright verifies save path and cleanup behavior.');

            await page.getByRole('button', { name: 'Save Draft' }).click();
            await expect(page).toHaveURL(/\/incident-management\/incidents\/[0-9a-fA-F-]{36}$/);

            createdId = page.url().split('/').pop();
            expect(createdId).toBeTruthy();

            await gotoAndStabilize(page, '/incident-management/incidents');
            await page.getByPlaceholder('Search incidents...').fill(jobTag);
            await page.locator('button:has(.pi-search)').first().click();
            await expect(page.locator('.p-datatable-tbody > tr', { hasText: jobTag })).toHaveCount(1);

            expectNoRuntimeErrors(runtimeErrors);
        } finally {
            if (!KEEP_TEST_DATA) {
                if (createdId) {
                    await deleteIncidentById(request, createdId);
                }
                await cleanupIncidentsByJobTag(request, jobTag);
            }
        }
    });
});
