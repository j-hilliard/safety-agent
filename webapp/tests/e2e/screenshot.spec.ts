import { test, expect } from './fixtures';
import {
    acceptPrimeConfirmDialog,
    expectNoRuntimeErrors,
    gotoAndStabilize,
    selectFirstPrimeDropdownOption,
    selectPrimeDropdownOption,
} from './utils/ui';

const KEEP_TEST_DATA = process.env.PW_KEEP_TEST_DATA === 'true';

async function deleteRowByText(page, text: string) {
    const row = page.locator('.p-datatable-tbody > tr', { hasText: text }).first();
    if (await row.count()) {
        await row.locator('button:has(.pi-trash)').click();
        await acceptPrimeConfirmDialog(page);
    }
}

test.describe('App navigation and button coverage', () => {
    test.describe.configure({ timeout: 120_000 });

    test('cross-app navigation from dashboard and menu links works', async ({ page, runtimeErrors }) => {
        await gotoAndStabilize(page, '/dashboard');

        await expect(page.getByRole('heading', { name: 'Stronghold BizApps Suite' })).toBeVisible();
        await expect(page.getByRole('link', { name: 'Incident Management' })).toBeVisible();
        await expect(page.getByRole('link', { name: 'Project Management System' })).toBeVisible();

        await page.locator('a[href=\"/incident-management\"]').first().click();
        await expect(page).toHaveURL(/\/incident-management\/incidents$/);

        const menu = page.locator('.layout-menu-container');
        await menu.getByRole('link', { name: 'New Incident' }).click();
        await expect(page).toHaveURL(/\/incident-management\/incidents\/new$/);

        await menu.getByRole('link', { name: 'Investigations' }).click();
        await expect(page).toHaveURL(/\/incident-management\/investigations$/);
        await expect(page.getByText('Investigation module coming soon.')).toBeVisible();

        await page.locator('#app-logo').click();
        await expect(page).toHaveURL(/\/dashboard$/);

        expectNoRuntimeErrors(runtimeErrors);
    });

    test('reference table maintenance tabs support create, edit, and delete flows', async ({ page, runtimeErrors }) => {
        const suffix = `${Date.now()}`;
        const companyCode = `PW${suffix.slice(-3)}`;
        const companyName = `Playwright Testing Company ${suffix}`;
        const updatedCompanyName = `Playwright Updated Company ${suffix}`;
        const regionName = `Playwright Region ${suffix}`;
        const lookupCode = `pw-item-${suffix.slice(-4)}`;
        const lookupName = `Playwright Lookup Item ${suffix}`;
        let shouldCleanup = true;

        try {
            await gotoAndStabilize(page, '/incident-management/ref-tables');

            await expect(page.getByRole('heading', { name: 'Reference Table Maintenance' })).toBeVisible();

            await page.getByRole('button', { name: 'Add Company' }).click();
            const companyDialog = page.getByRole('dialog', { name: 'Add Company' });
            await companyDialog.locator('input[placeholder="e.g. CCI"]').fill(companyCode);
            await companyDialog.locator('input[placeholder="Company name"]').fill(companyName);
            await companyDialog.getByRole('button', { name: 'Save' }).click();
            const companyRow = page.locator('.p-datatable-tbody > tr', { hasText: companyName }).first();
            await expect(companyRow).toBeVisible();

            await companyRow.locator('button:has(.pi-pencil)').click();
            const editCompanyDialog = page.getByRole('dialog', { name: 'Edit Company' });
            await editCompanyDialog.locator('input[placeholder="Company name"]').fill(updatedCompanyName);
            await editCompanyDialog.getByRole('button', { name: 'Save' }).click();
            await expect(page.locator('.p-datatable-tbody > tr', { hasText: updatedCompanyName })).toBeVisible();

            const updatedCompanyRow = page.locator('.p-datatable-tbody > tr', { hasText: updatedCompanyName }).first();
            await updatedCompanyRow.locator('button:has(.pi-trash)').click();
            await acceptPrimeConfirmDialog(page);
            await expect(page.locator('.p-datatable-tbody > tr', { hasText: updatedCompanyName })).toHaveCount(0);

            await page.getByRole('tab', { name: 'Regions' }).click();
            await page.getByRole('button', { name: 'Add Region' }).click();
            const regionDialog = page.getByRole('dialog', { name: 'Add Region' });
            await selectPrimeDropdownOption(page, regionDialog.locator('.p-dropdown').first(), 'Central Construction Inc');
            await regionDialog.locator('input[placeholder="e.g. SE"]').fill(`PW${suffix.slice(-2)}`);
            await regionDialog.locator('input[placeholder="Region name"]').fill(regionName);
            await regionDialog.getByRole('button', { name: 'Save' }).click();
            const regionRow = page.locator('.p-datatable-tbody > tr', { hasText: regionName }).first();
            await expect(regionRow).toBeVisible();

            await regionRow.locator('button:has(.pi-trash)').click();
            await acceptPrimeConfirmDialog(page);
            await expect(page.locator('.p-datatable-tbody > tr', { hasText: regionName })).toHaveCount(0);

            await page.getByRole('tab', { name: 'Lookup Tables' }).click();
            await page.getByRole('button', { name: 'Add Item' }).click();
            const lookupDialog = page.getByRole('dialog', { name: 'Add Item' });
            await selectFirstPrimeDropdownOption(page, lookupDialog.locator('.p-dropdown').first());
            await lookupDialog.locator('input[placeholder="e.g. spill"]').fill(lookupCode);
            await lookupDialog.locator('input[placeholder="Display name"]').fill(lookupName);
            await lookupDialog.getByRole('button', { name: 'Save' }).click();
            const lookupRow = page.locator('.p-datatable-tbody > tr', { hasText: lookupName }).first();
            await expect(lookupRow).toBeVisible();

            await lookupRow.locator('button:has(.pi-trash)').click();
            await acceptPrimeConfirmDialog(page);
            await expect(page.locator('.p-datatable-tbody > tr', { hasText: lookupName })).toHaveCount(0);

            shouldCleanup = false;
            expectNoRuntimeErrors(runtimeErrors);
        } finally {
            if (shouldCleanup && !KEEP_TEST_DATA) {
                try {
                    await gotoAndStabilize(page, '/incident-management/ref-tables');
                    await deleteRowByText(page, updatedCompanyName);
                    await deleteRowByText(page, companyName);
                    await page.getByRole('tab', { name: 'Regions' }).click();
                    await deleteRowByText(page, regionName);
                    await page.getByRole('tab', { name: 'Lookup Tables' }).click();
                    await deleteRowByText(page, lookupName);
                } catch {
                    // Ignore cleanup fallback failures if the browser context was already closed.
                }
            }
        }
    });

    test('project dashboard buttons cover create, view, edit, toggle, and delete', async ({ page, runtimeErrors }) => {
        const suffix = `${Date.now()}`;
        const projectName = `Playwright Project ${suffix}`;
        let shouldCleanup = true;

        try {
            await gotoAndStabilize(page, '/project-management-system/dashboard');

            await expect(page.getByRole('heading', { name: 'Project Management System' })).toBeVisible();

            await page.getByRole('button', { name: 'Create Project' }).click();
            const createDialog = page.getByRole('dialog', { name: 'Create Project' });
            await createDialog.locator('input.p-inputtext').nth(0).fill(projectName);
            await selectPrimeDropdownOption(page, createDialog.locator('.p-dropdown').first(), 'Active');
            await createDialog.locator('input.p-inputtext').nth(1).fill('QA Owner');
            await createDialog.locator('.p-calendar input').fill('03/13/2026');
            await createDialog.getByRole('button', { name: 'Create' }).click();

            const row = page.locator('.p-datatable-tbody > tr', { hasText: projectName }).first();
            await expect(row).toBeVisible();

            await row.locator('button:has(.pi-eye)').click();
            await expect(page.getByRole('dialog', { name: 'Edit Project' })).toBeVisible();
            await page.getByRole('dialog', { name: 'Edit Project' }).getByRole('button', { name: 'Cancel' }).click();

            await row.locator('button:has(.pi-pencil)').click();
            const editDialog = page.getByRole('dialog', { name: 'Edit Project' });
            await editDialog.locator('input.p-inputtext').nth(1).fill('QA Owner Updated');
            await editDialog.getByRole('button', { name: 'Update' }).click();
            await expect(page.locator('.p-datatable-tbody > tr', { hasText: 'QA Owner Updated' })).toBeVisible();

            const updatedRow = page.locator('.p-datatable-tbody > tr', { hasText: projectName }).first();
            await updatedRow.locator('button:has(.pi-ban)').click();
            await page.getByRole('dialog', { name: 'Confirm Disable' }).getByRole('button', { name: 'Disable' }).click();
            await expect(updatedRow.locator('.p-tag-value')).toHaveText('Disabled');

            await updatedRow.locator('button:has(.pi-check-circle)').click();
            await page.getByRole('dialog', { name: 'Confirm Activate' }).getByRole('button', { name: 'Activate' }).click();
            await expect(updatedRow.locator('.p-tag-value')).toHaveText('Active');

            await updatedRow.locator('button:has(.pi-ban)').click();
            await page.getByRole('dialog', { name: 'Confirm Disable' }).getByRole('button', { name: 'Disable' }).click();
            await updatedRow.locator('button:has(.pi-trash)').click();
            await page.getByRole('dialog', { name: 'Confirm Delete' }).getByRole('button', { name: 'Delete' }).click();
            await expect(page.locator('.p-datatable-tbody > tr', { hasText: projectName })).toHaveCount(0);

            shouldCleanup = false;
            expectNoRuntimeErrors(runtimeErrors);
        } finally {
            if (shouldCleanup && !KEEP_TEST_DATA) {
                try {
                    await gotoAndStabilize(page, '/project-management-system/dashboard');
                    const row = page.locator('.p-datatable-tbody > tr', { hasText: projectName }).first();
                    if (await row.count()) {
                        if ((await row.locator('.p-tag-value').textContent())?.trim() === 'Active') {
                            await row.locator('button:has(.pi-ban)').click();
                            await page.getByRole('dialog', { name: 'Confirm Disable' }).getByRole('button', { name: 'Disable' }).click();
                        }
                        const staleRow = page.locator('.p-datatable-tbody > tr', { hasText: projectName }).first();
                        await staleRow.locator('button:has(.pi-trash)').click();
                        await page.getByRole('dialog', { name: 'Confirm Delete' }).getByRole('button', { name: 'Delete' }).click();
                    }
                } catch {
                    // Ignore cleanup fallback failures if the browser context was already closed.
                }
            }
        }
    });
});
