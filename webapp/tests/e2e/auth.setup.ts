import { test as setup, expect } from '@playwright/test';
import path from 'path';

const authFile = path.join(__dirname, '.auth/user.json');

// Run this once to capture your Azure AD session. The browser will open
// and wait for you to be fully logged in, then saves the auth state.
setup('authenticate', async ({ page }) => {
    await page.goto('/');

    // Wait until we're past the Azure AD login — the nav or main content must appear
    await page.waitForSelector('nav, [data-testid="app-shell"], .p-menubar, aside', {
        timeout: 120_000, // 2 min to handle interactive login
    });

    // Make sure we're on an authenticated page
    await expect(page).not.toHaveURL(/login\.microsoftonline\.com/);

    await page.context().storageState({ path: authFile });
    console.log(`Auth state saved to ${authFile}`);
});
