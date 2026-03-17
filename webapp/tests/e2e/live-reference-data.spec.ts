import { test, expect } from '@playwright/test';
import { API_BASE_URL, getJsonOrFail } from './utils/liveApi';

const expectedCompanies = [
    'Catalyst Changers Inc.',
    'Cat-Spec Ltd.',
    'Elite Turnaround Specialists Ltd.',
    'Stronghold University',
    'Stronghold Inspections Ltd.',
    'Stronghold Tower Group',
    'Stronghold Tank Services Ltd.',
    'Turnkey I&E Ltd.',
];

const expectedRegions = [
    'Broken Arrow, OK',
    'Chippewa Falls, WI',
    'Concord, CA',
    'Edmonton, AB',
    'Laporte, TX',
    'Long Beach, CA',
    'Lucedale, MS',
    'Mickleton, NJ',
    'Nederland, TX',
    'Ogden, UT',
    'Oregon, OH',
    'Point Lisas, TT',
    'Reserve, LA',
    'Robstown, TX',
    'Sarnia, ON',
    'Sulphur, LA',
    'Sundre, AB',
];

type RefCompanyDto = {
    id?: string;
    name?: string;
};

type RefRegionDto = {
    id?: string;
    name?: string;
};

test.describe('Live Backend Reference Data (No Mocks)', () => {
    test('companies and regions endpoints return expected seed values', async ({ request }) => {
        const companies = await getJsonOrFail<RefCompanyDto[]>(request, `${API_BASE_URL}/v1/ReferenceData/companies`);
        const regions = await getJsonOrFail<RefRegionDto[]>(request, `${API_BASE_URL}/v1/ReferenceData/regions`);

        expect(companies.length).toBeGreaterThanOrEqual(expectedCompanies.length);
        expect(regions.length).toBeGreaterThanOrEqual(expectedRegions.length);

        const companyNames = companies.map((x) => x.name).filter(Boolean) as string[];
        const regionNames = regions.map((x) => x.name).filter(Boolean) as string[];

        for (const name of expectedCompanies) {
            expect(companyNames).toContain(name);
        }

        for (const name of expectedRegions) {
            expect(regionNames).toContain(name);
        }
    });

    test('new incident form shows company and region dropdown options from live API', async ({ page, request }) => {
        await getJsonOrFail<RefCompanyDto[]>(request, `${API_BASE_URL}/v1/ReferenceData/companies`);
        await getJsonOrFail<RefRegionDto[]>(request, `${API_BASE_URL}/v1/ReferenceData/regions`);

        await page.goto('/incident-management/incidents/new');
        await expect(page.getByRole('heading', { name: 'New Incident Report' })).toBeVisible();

        const companyDropdown = page.locator('.p-dropdown').first();
        await companyDropdown.click();

        const companyOptions = page.locator('.p-dropdown-panel .p-dropdown-item');
        await expect.poll(async () => await companyOptions.count()).toBeGreaterThan(0);

        const companyLabels = (await companyOptions.allTextContents()).map((x) => x.trim()).filter(Boolean);
        for (const name of expectedCompanies) {
            expect(companyLabels).toContain(name);
        }

        const catalystOption = page.locator('.p-dropdown-panel .p-dropdown-item', { hasText: 'Catalyst Changers Inc.' }).first();
        if (await catalystOption.count()) {
            await catalystOption.click();
        } else {
            await companyOptions.first().click();
        }

        const regionDropdown = page.locator('.p-dropdown').nth(1);
        await expect(regionDropdown).not.toHaveClass(/p-disabled/);
        await regionDropdown.click();

        const regionOptions = page.locator('.p-dropdown-panel .p-dropdown-item');
        await expect.poll(async () => await regionOptions.count()).toBeGreaterThan(0);

        const regionLabels = (await regionOptions.allTextContents()).map((x) => x.trim()).filter(Boolean);

        if (await catalystOption.count()) {
            expect(regionLabels).toContain('Edmonton, AB');
            expect(regionLabels).toContain('Sundre, AB');
        }
    });
});