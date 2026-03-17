import { test, expect } from './fixtures';
import {
    acceptPrimeConfirmDialog,
    expectNoRuntimeErrors,
    gotoAndStabilize,
    selectFirstPrimeDropdownOption,
} from './utils/ui';

const KEEP_TEST_DATA = process.env.PW_KEEP_TEST_DATA === 'true';

async function deleteIncidentByJobTag(page, jobTag: string) {
    try {
        await gotoAndStabilize(page, '/incident-management/incidents');
        await page.getByPlaceholder('Search incidents...').fill(jobTag);
        await page.locator('button:has(.pi-search)').first().click();

        const row = page.locator('.p-datatable-tbody > tr', { hasText: jobTag }).first();
        if (await row.count()) {
            await row.locator('button:has(.pi-trash)').click();
            await acceptPrimeConfirmDialog(page);
        }
    } catch {
        // Ignore cleanup fallback failures if the browser context was already closed.
    }
}

test.describe('Incident Management workflows', () => {
    test.describe.configure({ timeout: 120_000 });

    test('incident list supports search, new, edit, and delete actions', async ({ page, runtimeErrors }) => {
        const jobTag = `PW-LIST-${Date.now()}`;
        let shouldCleanup = true;

        try {
            await gotoAndStabilize(page, '/incident-management/incidents');
            await expect(page.getByRole('heading', { name: 'Incidents' })).toBeVisible();
            await expect(page.getByPlaceholder('Search incidents...')).toBeVisible();

            await page.getByRole('button', { name: 'New Incident' }).click();
            await expect(page).toHaveURL(/\/incident-management\/incidents\/new$/);

            await selectFirstPrimeDropdownOption(page, page.locator('.p-dropdown').first());
            await page.locator('xpath=//label[contains(normalize-space(.), "Job Number")]/following::input[1]').fill(jobTag);
            await page.getByPlaceholder('Provide a detailed description of the incident').fill(`Created by ${jobTag}`);

            await page.getByRole('button', { name: 'Save Draft' }).click();
            await expect(page).toHaveURL(/\/incident-management\/incidents\/.+/);
            const createdId = page.url().split('/').pop();

            await gotoAndStabilize(page, '/incident-management/incidents');
            await page.getByPlaceholder('Search incidents...').fill(jobTag);
            await page.locator('button:has(.pi-search)').first().click();

            const row = page.locator('.p-datatable-tbody > tr', { hasText: jobTag }).first();
            await expect(row).toBeVisible();

            await row.locator('button:has(.pi-pencil)').click();
            await expect(page).toHaveURL(new RegExp(`/incident-management/incidents/${createdId}$`));

            await gotoAndStabilize(page, '/incident-management/incidents');
            await page.getByPlaceholder('Search incidents...').fill(jobTag);
            await page.locator('button:has(.pi-search)').first().click();
            await row.locator('button:has(.pi-trash)').click();
            await acceptPrimeConfirmDialog(page);
            await expect(page.locator('.p-datatable-tbody > tr', { hasText: jobTag })).toHaveCount(0);

            shouldCleanup = false;
            expectNoRuntimeErrors(runtimeErrors);
        } finally {
            if (shouldCleanup && !KEEP_TEST_DATA) {
                await deleteIncidentByJobTag(page, jobTag);
            }
        }
    });

    test('new incident form supports key buttons and submit flow', async ({ page, runtimeErrors }) => {
        const jobTag = `PW-SUBMIT-${Date.now()}`;
        let shouldCleanup = true;

        try {
            await gotoAndStabilize(page, '/incident-management/incidents/new');

            await expect(page.getByRole('heading', { name: 'New Incident Report' })).toBeVisible();

            const companyDropdown = page.locator('.p-dropdown').first();
            await selectFirstPrimeDropdownOption(page, companyDropdown);

            const regionDropdown = page.locator('.p-dropdown').nth(1);
            await expect(regionDropdown).not.toHaveClass(/p-disabled/);
            await selectFirstPrimeDropdownOption(page, regionDropdown);

            const actualSeverity = page.locator('input[id^="sa-"]').nth(1);
            await page.locator('.p-radiobutton:has(input[id^="sa-"])').nth(1).click();
            await expect(actualSeverity).toBeChecked();

            const potentialSeverity = page.locator('input[id^="sp-"]').nth(2);
            await page.locator('.p-radiobutton:has(input[id^="sp-"])').nth(2).click();
            await expect(potentialSeverity).toBeChecked();

            await page.locator('.p-radiobutton:has(#ic-NearMiss)').click();
            await expect(page.locator('#ic-NearMiss')).toBeChecked();

            await page.locator('xpath=//label[contains(normalize-space(.), "Job Number")]/following::input[1]').fill(jobTag);
            await page.getByPlaceholder('Describe the work being performed at the time of the incident').fill('Performing scheduled inspection work.');
            await page.getByPlaceholder('Provide a detailed description of the incident').fill('Employee slipped while stepping off a platform and reported soreness.');

            const employeeCard = page.locator('.p-card').nth(3);
            const employeeRows = employeeCard.locator('.p-datatable-tbody > tr');
            await page.getByRole('button', { name: 'Add Employee' }).click();
            await expect(employeeRows).toHaveCount(1);
            await page.getByPlaceholder('ID', { exact: true }).fill('E-771');
            await page.getByPlaceholder('Full name', { exact: true }).fill('Taylor Rivera');
            await employeeRows.first().locator('button:has(.pi-trash)').click();
            await expect(employeeCard.getByPlaceholder('ID', { exact: true })).toHaveCount(0);

            await page.getByRole('button', { name: 'Save Draft' }).click();
            await expect(page).toHaveURL(/\/incident-management\/incidents\/.+/);

            await page.getByRole('button', { name: 'Submit' }).click();
            await expect(page).toHaveURL(/\/incident-management\/incidents$/);

            await page.getByPlaceholder('Search incidents...').fill(jobTag);
            await page.locator('button:has(.pi-search)').first().click();
            await expect(page.locator('.p-datatable-tbody > tr', { hasText: jobTag })).toHaveCount(1);

            await page.getByRole('button', { name: 'New Incident' }).click();
            await expect(page).toHaveURL(/\/incident-management\/incidents\/new$/);

            await page.getByRole('button', { name: 'Cancel' }).click();
            await expect(page).toHaveURL(/\/incident-management\/incidents$/);

            await page.getByPlaceholder('Search incidents...').fill(jobTag);
            await page.locator('button:has(.pi-search)').first().click();
            const newIncidentRow = page.locator('.p-datatable-tbody > tr', { hasText: jobTag }).first();
            await newIncidentRow.locator('button:has(.pi-trash)').click();
            await acceptPrimeConfirmDialog(page);
            await expect(page.locator('.p-datatable-tbody > tr', { hasText: jobTag })).toHaveCount(0);

            shouldCleanup = false;
            expectNoRuntimeErrors(runtimeErrors);
        } finally {
            if (shouldCleanup && !KEEP_TEST_DATA) {
                await deleteIncidentByJobTag(page, jobTag);
            }
        }
    });

    test('reference card controls are interactive on new incident form', async ({ page, runtimeErrors }) => {
        await gotoAndStabilize(page, '/incident-management/incidents/new');

        const cardTitles = page.locator('.p-card .p-card-title');
        await expect(cardTitles).toContainText([
            'Initial Report',
            'Initial Incident Details',
            'Incident Description Summary',
            'Involved Employees',
        ]);

        await page.getByRole('button', { name: 'Add Employee' }).click();
        await expect(page.locator('.p-datatable-tbody > tr')).toHaveCount(1);

        await selectFirstPrimeDropdownOption(page, page.locator('.p-dropdown').first());

        await page.locator('label[for="ref-li-1"]').click();
        await expect(page.locator('#ref-li-1')).toBeChecked();

        expectNoRuntimeErrors(runtimeErrors);
    });
});
