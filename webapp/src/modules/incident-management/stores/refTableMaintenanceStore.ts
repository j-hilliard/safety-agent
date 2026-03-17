import { defineStore } from 'pinia';
import { ref } from 'vue';
import { useToast } from 'primevue/usetoast';
import { useApiStore } from '@/stores/apiStore';
import type { RefCompanyDto, RefRegionDto, RefOptionDto } from '@/apiclient/client';

export interface RefReferenceTypeDto {
    id: string;
    code: string;
    name: string;
    appliesTo: string;
}

export const useRefTableMaintenanceStore = defineStore('refTableMaintenance', () => {
    const apiStore = useApiStore();
    const toast = useToast();

    const companies = ref<RefCompanyDto[]>([]);
    const regions = ref<RefRegionDto[]>([]);
    const referenceTypes = ref<RefReferenceTypeDto[]>([]);
    const lookupItems = ref<RefOptionDto[]>([]);
    const loading = ref(false);
    const saving = ref(false);

    function api() { return apiStore.api; }
    function baseUrl() { return `${apiStore.api.defaults.baseURL}/v1/ReferenceData`; }

    // --- Companies ---
    async function loadCompanies() {
        loading.value = true;
        try {
            const res = await api().get<RefCompanyDto[]>(`${baseUrl()}/companies`);
            companies.value = res.data;
        } finally {
            loading.value = false;
        }
    }

    async function saveCompany(data: { id?: string; code: string; name: string; isActive: boolean }): Promise<boolean> {
        saving.value = true;
        try {
            if (data.id) {
                const res = await api().put<RefCompanyDto>(`${baseUrl()}/companies/${data.id}`, data);
                const idx = companies.value.findIndex(c => c.id === data.id);
                if (idx !== -1) companies.value[idx] = res.data;
            } else {
                const res = await api().post<RefCompanyDto>(`${baseUrl()}/companies`, data);
                companies.value.push(res.data);
            }
            toast.add({ severity: 'success', summary: 'Saved', detail: 'Company saved.', life: 3000 });
            return true;
        } catch {
            toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to save company.', life: 4000 });
            return false;
        } finally {
            saving.value = false;
        }
    }

    async function deleteCompany(id: string): Promise<boolean> {
        try {
            await api().delete(`${baseUrl()}/companies/${id}`);
            companies.value = companies.value.filter(c => c.id !== id);
            toast.add({ severity: 'success', summary: 'Deleted', detail: 'Company deactivated.', life: 3000 });
            return true;
        } catch {
            toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete company.', life: 4000 });
            return false;
        }
    }

    // --- Regions ---
    async function loadRegions() {
        loading.value = true;
        try {
            const res = await api().get<RefRegionDto[]>(`${baseUrl()}/regions`);
            regions.value = res.data;
        } finally {
            loading.value = false;
        }
    }

    async function saveRegion(data: { id?: string; companyId?: string; code: string; name: string; isActive: boolean }): Promise<boolean> {
        saving.value = true;
        try {
            if (data.id) {
                const res = await api().put<RefRegionDto>(`${baseUrl()}/regions/${data.id}`, data);
                const idx = regions.value.findIndex(r => r.id === data.id);
                if (idx !== -1) regions.value[idx] = res.data;
            } else {
                const res = await api().post<RefRegionDto>(`${baseUrl()}/regions`, data);
                regions.value.push(res.data);
            }
            toast.add({ severity: 'success', summary: 'Saved', detail: 'Region saved.', life: 3000 });
            return true;
        } catch {
            toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to save region.', life: 4000 });
            return false;
        } finally {
            saving.value = false;
        }
    }

    async function deleteRegion(id: string): Promise<boolean> {
        try {
            await api().delete(`${baseUrl()}/regions/${id}`);
            regions.value = regions.value.filter(r => r.id !== id);
            toast.add({ severity: 'success', summary: 'Deleted', detail: 'Region deactivated.', life: 3000 });
            return true;
        } catch {
            toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete region.', life: 4000 });
            return false;
        }
    }

    // --- Reference Types + Lookup Items ---
    async function loadReferenceTypes() {
        const res = await api().get<RefReferenceTypeDto[]>(`${baseUrl()}/reference-types`);
        referenceTypes.value = res.data;
    }

    async function loadLookupItems() {
        loading.value = true;
        try {
            const res = await api().get<Record<string, RefOptionDto[]>>(`${baseUrl()}/incident-options`);
            lookupItems.value = Object.values(res.data).flat();
        } finally {
            loading.value = false;
        }
    }

    async function saveLookupItem(data: { id?: string; referenceTypeId: string; code: string; name: string; isActive: boolean }): Promise<boolean> {
        saving.value = true;
        try {
            if (data.id) {
                const res = await api().put<RefOptionDto>(`${baseUrl()}/lookup-items/${data.id}`, data);
                const idx = lookupItems.value.findIndex(i => i.id === data.id);
                if (idx !== -1) lookupItems.value[idx] = res.data;
            } else {
                const res = await api().post<RefOptionDto>(`${baseUrl()}/lookup-items`, data);
                lookupItems.value.push(res.data);
            }
            toast.add({ severity: 'success', summary: 'Saved', detail: 'Item saved.', life: 3000 });
            return true;
        } catch {
            toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to save item.', life: 4000 });
            return false;
        } finally {
            saving.value = false;
        }
    }

    async function deleteLookupItem(id: string): Promise<boolean> {
        try {
            await api().delete(`${baseUrl()}/lookup-items/${id}`);
            lookupItems.value = lookupItems.value.filter(i => i.id !== id);
            toast.add({ severity: 'success', summary: 'Deleted', detail: 'Item deactivated.', life: 3000 });
            return true;
        } catch {
            toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete item.', life: 4000 });
            return false;
        }
    }

    return {
        companies, regions, referenceTypes, lookupItems, loading, saving,
        loadCompanies, saveCompany, deleteCompany,
        loadRegions, saveRegion, deleteRegion,
        loadReferenceTypes, loadLookupItems, saveLookupItem, deleteLookupItem,
    };
});
