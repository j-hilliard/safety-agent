import { expect, Locator, Page } from '@playwright/test';

export function startRuntimeErrorTracking(page: Page): string[] {
    const errors: string[] = [];

    page.on('pageerror', (error) => {
        errors.push(`[pageerror] ${error.message}`);
    });

    page.on('console', (msg) => {
        if (msg.type() !== 'error') {
            return;
        }

        const text = msg.text();
        if (text.includes('favicon.ico') || text.includes('ResizeObserver loop limit exceeded')) {
            return;
        }

        errors.push(`[console] ${text}`);
    });

    return errors;
}

export async function gotoAndStabilize(page: Page, path: string) {
    await page.goto(path);
    await page.waitForLoadState('networkidle');
    await disableAnimations(page);
}

export async function disableAnimations(page: Page) {
    await page.addStyleTag({
        content: `
            *, *::before, *::after {
                transition: none !important;
                animation: none !important;
                caret-color: transparent !important;
            }
            .p-toast, .p-tooltip, #nprogress, .p-ink {
                display: none !important;
            }
        `,
    });
}

export function expectNoRuntimeErrors(runtimeErrors: string[]) {
    expect(runtimeErrors, runtimeErrors.join('\n')).toEqual([]);
}

export async function acceptPrimeConfirmDialog(page: Page) {
    const dialog = page.locator('.p-confirm-dialog').last();
    await expect(dialog).toBeVisible();
    await dialog.locator('.p-confirm-dialog-accept').click({ force: true });
    try {
        await expect(dialog).toBeHidden({ timeout: 3_000 });
    } catch {
        const closeButton = dialog.locator('.p-dialog-header-close');
        if (await closeButton.count()) {
            await closeButton.click({ force: true });
        } else {
            await page.keyboard.press('Escape');
        }
        await page.waitForTimeout(150);
    }

    for (let i = 0; i < 4; i += 1) {
        const overlayMasks = page.locator('.p-dialog-mask.p-component-overlay:visible');
        if ((await overlayMasks.count()) === 0) {
            break;
        }

        const closeButtons = page.locator('.p-dialog-header-close:visible');
        if (await closeButtons.count()) {
            await closeButtons.first().click({ force: true });
        } else {
            await page.keyboard.press('Escape');
        }
        await page.waitForTimeout(100);
    }
}

export async function selectPrimeDropdownOption(page: Page, dropdown: Locator, optionText: string) {
    await dropdown.click();
    await page.locator('.p-dropdown-item', { hasText: optionText }).first().click();
}

export async function selectFirstPrimeDropdownOption(page: Page, dropdown: Locator) {
    await dropdown.click();
    const first = page.locator('.p-dropdown-item').first();
    await expect(first).toBeVisible();
    await first.click();
}
