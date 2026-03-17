import { defineStore } from 'pinia';
import { ref } from 'vue';
import { useApiStore } from '@/stores/apiStore';
import { ReferenceDataClient, RefCompanyDto, RefRegionDto, RefSeverityDto, RefOptionDto, RefWorkflowStateDto } from '@/apiclient/client';

export const useReferenceDataStore = defineStore('incidentReferenceData', () => {
    const apiStore = useApiStore();

    const companies = ref<RefCompanyDto[]>([]);
    const regions = ref<RefRegionDto[]>([]);
    const severities = ref<RefSeverityDto[]>([]);
    const incidentOptions = ref<Record<string, RefOptionDto[]>>({});
    const workflowStates = ref<RefWorkflowStateDto[]>([]);

    const loading = ref(false);
    const initialized = ref(false);

    function getClient() {
        return new ReferenceDataClient(apiStore.api.defaults.baseURL, apiStore.api);
    }

    async function loadAll() {
        if (initialized.value) return;
        loading.value = true;
        try {
            const client = getClient();
            const [companiesResult, severitiesResult, optionsResult, workflowStatesResult] = await Promise.allSettled([
                client.getCompanies(),
                client.getSeverities(),
                client.getIncidentReferenceOptions(),
                client.getWorkflowStates(undefined),
            ]);

            if (companiesResult.status === 'fulfilled') {
                companies.value = companiesResult.value;
            }

            if (severitiesResult.status === 'fulfilled') {
                severities.value = severitiesResult.value;
            }

            if (optionsResult.status === 'fulfilled') {
                incidentOptions.value = optionsResult.value as Record<string, RefOptionDto[]>;
            }

            if (workflowStatesResult.status === 'fulfilled') {
                workflowStates.value = workflowStatesResult.value;
            }

            initialized.value =
                companiesResult.status === 'fulfilled'
                && severitiesResult.status === 'fulfilled'
                && optionsResult.status === 'fulfilled'
                && workflowStatesResult.status === 'fulfilled';
        } finally {
            loading.value = false;
        }
    }

    async function loadRegions(companyId?: string) {
        const client = getClient();
        try {
            regions.value = await client.getRegions(companyId ?? null);

            // If company-specific mapping is incomplete, ensure users still see region options.
            if (companyId && regions.value.length === 0) {
                regions.value = await client.getRegions(null);
            }
        } catch {
            regions.value = [];
        }
    }

    function getOptionsByType(typeCode: string): RefOptionDto[] {
        return incidentOptions.value[typeCode] ?? [];
    }

    return {
        companies,
        regions,
        severities,
        incidentOptions,
        workflowStates,
        loading,
        initialized,
        loadAll,
        loadRegions,
        getOptionsByType,
    };
});
