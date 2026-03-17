import { defineConfig, devices } from '@playwright/test';

const isCI = !!process.env.CI;

export default defineConfig({
    testDir: './tests/e2e',
    timeout: 60_000,
    fullyParallel: false,
    forbidOnly: isCI,
    retries: isCI ? 1 : 0,
    workers: isCI ? 1 : undefined,
    reporter: [['list'], ['html', { open: 'never' }]],
    expect: {
        timeout: 10_000,
        toHaveScreenshot: {
            maxDiffPixelRatio: 0.02,
        },
    },
    use: {
        baseURL: 'https://localhost:7208',
        ignoreHTTPSErrors: true,
        trace: 'retain-on-failure',
        screenshot: 'only-on-failure',
        video: 'retain-on-failure',
        viewport: { width: 1536, height: 960 },
    },
    webServer: {
        command: 'npm run dev -- --host 127.0.0.1 --port 7208',
        url: 'https://localhost:7208',
        reuseExistingServer: !isCI,
        timeout: 120_000,
        ignoreHTTPSErrors: true,
        stdout: 'pipe',
        stderr: 'pipe',
    },
    projects: [
        {
            name: 'chromium',
            use: { ...devices['Desktop Chrome'] },
        },
    ],
});
