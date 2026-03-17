import { defineStore } from 'pinia';
import { ref } from 'vue';
import { useToast } from 'primevue/usetoast';
import { useApiStore } from '@/stores/apiStore';
import {
    IncidentReportClient,
    IncidentReport,
    IncidentReportListItem,
    IncidentEmployeeInvolved,
    IncidentAction,
} from '@/apiclient/client';

const EMPTY_FORM = (): IncidentReport => {
    const r = new IncidentReport();
    r.incidentDate = new Date();
    r.employeesInvolved = [];
    r.actions = [];
    r.referenceIds = [];
    return r;
};

export const useIncidentStore = defineStore('incident', () => {
    const apiStore = useApiStore();
    const toast = useToast();

    const incidents = ref<IncidentReportListItem[]>([]);
    const currentIncident = ref<IncidentReport | null>(null);
    const form = ref<IncidentReport>(EMPTY_FORM());
    const loading = ref(false);
    const saving = ref(false);

    // List filters
    const searchTerm = ref<string | undefined>(undefined);
    const filterCompanyId = ref<string | undefined>(undefined);
    const filterRegionId = ref<string | undefined>(undefined);
    const filterStatus = ref<string | undefined>(undefined);
    const filterDateFrom = ref<Date | undefined>(undefined);
    const filterDateTo = ref<Date | undefined>(undefined);

    function getClient() {
        return new IncidentReportClient(apiStore.api.defaults.baseURL, apiStore.api);
    }

    async function loadList() {
        loading.value = true;
        try {
            incidents.value = await getClient().getIncidentList(
                searchTerm.value ?? null,
                filterCompanyId.value ?? null,
                filterRegionId.value ?? null,
                filterStatus.value ?? null,
                filterDateFrom.value ?? null,
                filterDateTo.value ?? null,
            );
        } finally {
            loading.value = false;
        }
    }

    async function loadIncident(id: string) {
        loading.value = true;
        try {
            currentIncident.value = await getClient().getIncident(id) ?? null;
            if (currentIncident.value) {
                form.value = Object.assign(EMPTY_FORM(), currentIncident.value);
            }
        } finally {
            loading.value = false;
        }
    }

    async function createIncident(): Promise<IncidentReport | null> {
        saving.value = true;
        try {
            const created = await getClient().createIncident(form.value);
            toast.add({ severity: 'success', summary: 'Incident Created', detail: `Incident ${created.incidentNumber} created successfully.`, life: 3000 });
            return created;
        } catch {
            toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to create incident report.', life: 4000 });
            return null;
        } finally {
            saving.value = false;
        }
    }

    async function updateIncident(id: string): Promise<IncidentReport | null> {
        saving.value = true;
        try {
            const updated = await getClient().updateIncident(id, form.value);
            toast.add({ severity: 'success', summary: 'Incident Updated', detail: 'Incident report saved.', life: 3000 });
            return updated ?? null;
        } catch {
            toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to save incident report.', life: 4000 });
            return null;
        } finally {
            saving.value = false;
        }
    }

    async function deleteIncident(id: string): Promise<boolean> {
        try {
            await getClient().deleteIncident(id);
            incidents.value = incidents.value.filter(i => i.id !== id);
            toast.add({ severity: 'success', summary: 'Deleted', detail: 'Incident report deleted.', life: 3000 });
            return true;
        } catch {
            toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete incident report.', life: 4000 });
            return false;
        }
    }

    function resetForm() {
        form.value = EMPTY_FORM();
        currentIncident.value = null;
    }

    function addEmployee() {
        const emp = new IncidentEmployeeInvolved();
        form.value.employeesInvolved = [...(form.value.employeesInvolved ?? []), emp];
    }

    function removeEmployee(index: number) {
        form.value.employeesInvolved = (form.value.employeesInvolved ?? []).filter((_, i) => i !== index);
    }

    function addAction() {
        const action = new IncidentAction();
        form.value.actions = [...(form.value.actions ?? []), action];
    }

    function removeAction(index: number) {
        form.value.actions = (form.value.actions ?? []).filter((_, i) => i !== index);
    }

    function toggleReference(referenceId: string) {
        const ids = form.value.referenceIds ?? [];
        const idx = ids.indexOf(referenceId);
        if (idx === -1) {
            form.value.referenceIds = [...ids, referenceId];
        } else {
            form.value.referenceIds = ids.filter(id => id !== referenceId);
        }
    }

    function isReferenceSelected(referenceId: string): boolean {
        return (form.value.referenceIds ?? []).includes(referenceId);
    }

    return {
        incidents,
        currentIncident,
        form,
        loading,
        saving,
        searchTerm,
        filterCompanyId,
        filterRegionId,
        filterStatus,
        filterDateFrom,
        filterDateTo,
        loadList,
        loadIncident,
        createIncident,
        updateIncident,
        deleteIncident,
        resetForm,
        addEmployee,
        removeEmployee,
        addAction,
        removeAction,
        toggleReference,
        isReferenceSelected,
    };
});
