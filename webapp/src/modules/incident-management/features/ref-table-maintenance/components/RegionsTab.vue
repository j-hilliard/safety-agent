<template>
    <div>
        <div class="flex items-center justify-between mb-4">
            <Dropdown
                v-model="filterCompanyId"
                :options="store.companies"
                optionLabel="name"
                optionValue="id"
                placeholder="All Companies"
                showClear
                class="w-64"
                @change="store.loadRegions()"
            />
            <BaseButtonCreate label="Add Region" @click="openDialog()" />
        </div>

        <BaseDataTable :value="filteredRegions" :loading="store.loading" emptyMessage="No regions found.">
            <Column field="code" header="Code" sortable style="width: 120px" />
            <Column field="name" header="Name" sortable />
            <Column header="Company" sortable>
                <template #body="{ data }">
                    {{ companyName(data.companyId) }}
                </template>
            </Column>
            <Column field="isActive" header="Active" style="width: 80px">
                <template #body="{ data }">
                    <i :class="data.isActive ? 'pi pi-check text-green-400' : 'pi pi-times text-slate-500'" />
                </template>
            </Column>
            <Column header="Actions" style="width: 100px">
                <template #body="{ data }">
                    <BaseButtonIconEdit @click="openDialog(data)" />
                    <BaseButtonIconDelete @click="confirmDelete(data)" />
                </template>
            </Column>
        </BaseDataTable>

        <BaseDialog v-model:visible="dialogVisible" :header="editingId ? 'Edit Region' : 'Add Region'">
            <BaseFormField label="Company">
                <Dropdown
                    v-model="form.companyId"
                    :options="store.companies"
                    optionLabel="name"
                    optionValue="id"
                    placeholder="Select Company"
                    showClear
                    class="w-full"
                />
            </BaseFormField>
            <BaseFormField label="Code *">
                <InputText v-model="form.code" class="w-full" placeholder="e.g. SE" />
            </BaseFormField>
            <BaseFormField label="Name *">
                <InputText v-model="form.name" class="w-full" placeholder="Region name" />
            </BaseFormField>
            <div class="flex items-center gap-2 mt-2">
                <Checkbox v-model="form.isActive" :binary="true" inputId="re-active" />
                <label for="re-active" class="text-sm">Active</label>
            </div>
            <template #footer>
                <BaseButtonCancel @click="dialogVisible = false" />
                <BaseButtonSave :loading="store.saving" @click="save" />
            </template>
        </BaseDialog>

        <ConfirmDialog />
    </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useConfirm } from 'primevue/useconfirm';
import { useRefTableMaintenanceStore } from '@/modules/incident-management/stores/refTableMaintenanceStore';
import type { RefRegionDto } from '@/apiclient/client';
import BaseDataTable from '@/components/tables/BaseDataTable.vue';
import BaseDialog from '@/components/dialogs/BaseDialog.vue';
import BaseFormField from '@/components/forms/BaseFormField.vue';
import BaseButtonCreate from '@/components/buttons/BaseButtonCreate.vue';
import BaseButtonSave from '@/components/buttons/BaseButtonSave.vue';
import BaseButtonCancel from '@/components/buttons/BaseButtonCancel.vue';
import BaseButtonIconEdit from '@/components/buttons/BaseButtonIconEdit.vue';
import BaseButtonIconDelete from '@/components/buttons/BaseButtonIconDelete.vue';
import ConfirmDialog from 'primevue/confirmdialog';

const store = useRefTableMaintenanceStore();
const confirm = useConfirm();

const filterCompanyId = ref<string | undefined>(undefined);
const dialogVisible = ref(false);
const editingId = ref<string | undefined>(undefined);
const form = ref({ companyId: undefined as string | undefined, code: '', name: '', isActive: true });

onMounted(async () => {
    await Promise.all([store.loadCompanies(), store.loadRegions()]);
});

const filteredRegions = computed(() =>
    filterCompanyId.value
        ? store.regions.filter(r => r.companyId === filterCompanyId.value)
        : store.regions,
);

function companyName(companyId?: string): string {
    return store.companies.find(c => c.id === companyId)?.name ?? '—';
}

function openDialog(region?: RefRegionDto) {
    editingId.value = region?.id;
    form.value = {
        companyId: region?.companyId ?? undefined,
        code: region?.code ?? '',
        name: region?.name ?? '',
        isActive: region?.isActive ?? true,
    };
    dialogVisible.value = true;
}

async function save() {
    if (!form.value.code || !form.value.name) return;
    const ok = await store.saveRegion({ id: editingId.value, ...form.value });
    if (ok) dialogVisible.value = false;
}

function confirmDelete(region: RefRegionDto) {
    confirm.require({
        message: `Deactivate region "${region.name}"?`,
        header: 'Confirm',
        icon: 'pi pi-exclamation-triangle',
        acceptClass: 'p-button-danger',
        accept: () => store.deleteRegion(region.id!),
    });
}
</script>
