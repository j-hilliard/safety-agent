<template>
    <div>
        <div class="flex justify-end mb-4">
            <BaseButtonCreate label="Add Company" @click="openDialog()" />
        </div>

        <BaseDataTable :value="store.companies" :loading="store.loading" emptyMessage="No companies found.">
            <Column field="code" header="Code" sortable style="width: 120px" />
            <Column field="name" header="Name" sortable />
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

        <BaseDialog v-model:visible="dialogVisible" :header="editingId ? 'Edit Company' : 'Add Company'">
            <BaseFormField label="Code *">
                <InputText v-model="form.code" class="w-full" placeholder="e.g. CCI" />
            </BaseFormField>
            <BaseFormField label="Name *">
                <InputText v-model="form.name" class="w-full" placeholder="Company name" />
            </BaseFormField>
            <div class="flex items-center gap-2 mt-2">
                <Checkbox v-model="form.isActive" :binary="true" inputId="co-active" />
                <label for="co-active" class="text-sm">Active</label>
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
import { ref, onMounted } from 'vue';
import { useConfirm } from 'primevue/useconfirm';
import { useRefTableMaintenanceStore } from '@/modules/incident-management/stores/refTableMaintenanceStore';
import type { RefCompanyDto } from '@/apiclient/client';
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

const dialogVisible = ref(false);
const editingId = ref<string | undefined>(undefined);
const form = ref({ code: '', name: '', isActive: true });

onMounted(() => store.loadCompanies());

function openDialog(company?: RefCompanyDto) {
    editingId.value = company?.id;
    form.value = { code: company?.code ?? '', name: company?.name ?? '', isActive: company?.isActive ?? true };
    dialogVisible.value = true;
}

async function save() {
    if (!form.value.code || !form.value.name) return;
    const ok = await store.saveCompany({ id: editingId.value, ...form.value });
    if (ok) dialogVisible.value = false;
}

function confirmDelete(company: RefCompanyDto) {
    confirm.require({
        message: `Deactivate company "${company.name}"?`,
        header: 'Confirm',
        icon: 'pi pi-exclamation-triangle',
        acceptClass: 'p-button-danger',
        accept: () => store.deleteCompany(company.id!),
    });
}
</script>
