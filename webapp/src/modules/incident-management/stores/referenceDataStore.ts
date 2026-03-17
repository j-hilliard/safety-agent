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
            const [c, s, opts, ws] = await Promise.all([
                client.getCompanies(),
                client.getSeverities(),
                client.getIncidentReferenceOptions(),
                client.getWorkflowStates(undefined),
            ]);
            companies.value = c;
            severities.value = s;
            incidentOptions.value = opts as Record<string, RefOptionDto[]>;
            workflowStates.value = ws;
            initialized.value = true;
        } finally {
            loading.value = false;
        }
    }

    async function loadRegions(companyId?: string) {
        const client = getClient();
        regions.value = await client.getRegions(companyId ?? null);
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
