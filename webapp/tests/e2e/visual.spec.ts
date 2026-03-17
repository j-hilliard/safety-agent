import { test, expect } from './fixtures';
import { expectNoRuntimeErrors, gotoAndStabilize } from './utils/ui';

test.describe('Visual regression snapshots', () => {
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
