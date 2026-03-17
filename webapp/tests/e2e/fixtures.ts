import { test as base, expect } from '@playwright/test';
import { installMockApi, MockApiState } from './utils/mockApi';
import { startRuntimeErrorTracking } from './utils/ui';

type AppFixtures = {
    mockApi: MockApiState;
    runtimeErrors: string[];
};

export const test = base.extend<AppFixtures>({
    mockApi: [async ({ page }, use) => {
        const state = await installMockApi(page);
        await use(state);
    }, { auto: true }],
    runtimeErrors: [async ({ page }, use) => {
        const errors = startRuntimeErrorTracking(page);
        await use(errors);
    }, { auto: true }],
});

export { expect };
