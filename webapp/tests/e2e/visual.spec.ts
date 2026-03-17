import { test, expect } from './fixtures';
import { expectNoRuntimeErrors, gotoAndStabilize } from './utils/ui';

test.describe('Visual regression snapshots', () => {
    test('incident shell palette and row affordance are applied', async ({ page, runtimeErrors }) => {
        await gotoAndStabilize(page, '/incident-management/incidents');

        const topbarBg = await page.locator('.layout-topbar').evaluate((el) => getComputedStyle(el).backgroundColor);
        const sidebarBg = await page.locator('.layout-menu-container').evaluate((el) => getComputedStyle(el).backgroundColor);
        const firstRow = page.locator('.p-datatable-tbody > tr').first();
        await expect(firstRow).toBeVisible();
        const rowCursor = await firstRow.evaluate((el) => getComputedStyle(el).cursor);

        expect(topbarBg).toBe('rgb(13, 27, 56)');
        expect(sidebarBg).toBe('rgb(11, 23, 51)');
        expect(rowCursor).toBe('pointer');
        expectNoRuntimeErrors(runtimeErrors);
    });

    test('stronghold dashboard visual baseline', async ({ page, runtimeErrors }) => {
        await gotoAndStabilize(page, '/dashboard');
        await expect(page).toHaveScreenshot('dashboard-stronghold.png', { fullPage: true });
        expectNoRuntimeErrors(runtimeErrors);
    });

    test('incident list visual baseline', async ({ page, runtimeErrors }) => {
        await gotoAndStabilize(page, '/incident-management/incidents');
        await expect(page).toHaveScreenshot('incident-list.png', { fullPage: true });
        expectNoRuntimeErrors(runtimeErrors);
    });

    test('incident form visual baseline', async ({ page, runtimeErrors }) => {
        await gotoAndStabilize(page, '/incident-management/incidents/new');
        await expect(page).toHaveScreenshot('incident-form-new.png', { fullPage: true });
        expectNoRuntimeErrors(runtimeErrors);
    });

    test('reference tables visual baseline', async ({ page, runtimeErrors }) => {
        await gotoAndStabilize(page, '/incident-management/ref-tables');
        await expect(page).toHaveScreenshot('ref-tables.png', { fullPage: true });
        expectNoRuntimeErrors(runtimeErrors);
    });

    test('project dashboard visual baseline', async ({ page, runtimeErrors }) => {
        await gotoAndStabilize(page, '/project-management-system/dashboard');
        await expect(page).toHaveScreenshot('project-dashboard.png', { fullPage: true });
        expectNoRuntimeErrors(runtimeErrors);
    });
});
