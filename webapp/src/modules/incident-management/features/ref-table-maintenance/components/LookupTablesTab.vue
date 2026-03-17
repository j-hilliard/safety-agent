<template>
    <div>
        <div class="flex items-center justify-between mb-4">
            <Dropdown
                v-model="filterTypeId"
                :options="store.referenceTypes"
                optionLabel="name"
                optionValue="id"
                placeholder="All Categories"
                showClear
                class="w-72"
            />
            <BaseButtonCreate label="Add Item" @click="openDialog()" />
        </div>

        <BaseDataTable :value="filteredItems" :loading="store.loading" emptyMessage="No items found.">
            <Column header="Category" sortable>
                <template #body="{ data }">
                    {{ typeName(data.referenceTypeCode) }}
                </template>
            </Column>
            <Column field="code" header="Code" sortable style="width: 160px" />
            <Column field="name" header="Name" sortable />
            <Column header="Actions" style="width: 100px">
                <template #body="{ data }">
                    <BaseButtonIconEdit @click="openDialog(data)" />
                    <BaseButtonIconDelete @click="confirmDelete(data)" />
                </template>
            </Column>
        </BaseDataTable>

        <BaseDialog v-model:visible="dialogVisible" :header="editingId ? 'Edit Item' : 'Add Item'">
            <BaseFormField label="Category *">
                <Dropdown
                    v-model="form.referenceTypeId"
                    :options="store.referenceTypes"
                    optionLabel="name"
                    optionValue="id"
                    placeholder="Select Category"
                    class="w-full"
                />
            </BaseFormField>
            <BaseFormField label="Code *">
                <InputText v-model="form.code" class="w-full" placeholder="e.g. spill" />
            </BaseFormField>
            <BaseFormField label="Name *">
                <InputText v-model="form.name" class="w-full" placeholder="Display name" />
            </BaseFormField>
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
import type { RefOptionDto } from '@/apiclient/client';
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

const filterTypeId = ref<string | undefined>(undefined);
const dialogVisible = ref(false);
const editingId = ref<string | undefined>(undefined);
const form = ref({ referenceTypeId: '' as string, code: '', name: '', isActive: true });

onMounted(async () => {
    await Promise.all([store.loadReferenceTypes(), store.loadLookupItems()]);
});

const filteredItems = computed(() => {
    if (!filterTypeId.value) return store.lookupItems;
    const type = store.referenceTypes.find(t => t.id === filterTypeId.value);
    return type ? store.lookupItems.filter(i => i.referenceTypeCode === type.code) : store.lookupItems;
});

function typeName(typeCode?: string): string {
    return store.referenceTypes.find(t => t.code === typeCode)?.name ?? typeCode ?? '—';
}

function openDialog(item?: RefOptionDto) {
    editingId.value = item?.id;
    const typeId = item ? store.referenceTypes.find(t => t.code === item.referenceTypeCode)?.id ?? '' : '';
    form.value = { referenceTypeId: typeId, code: item?.code ?? '', name: item?.name ?? '', isActive: true };
    dialogVisible.value = true;
}

async function save() {
    if (!form.value.referenceTypeId || !form.value.code || !form.value.name) return;
    const ok = await store.saveLookupItem({ id: editingId.value, ...form.value });
    if (ok) dialogVisible.value = false;
}

function confirmDelete(item: RefOptionDto) {
    confirm.require({
        message: `Deactivate "${item.name}"?`,
        header: 'Confirm',
        icon: 'pi pi-exclamation-triangle',
        acceptClass: 'p-button-danger',
        accept: () => store.deleteLookupItem(item.id!),
    });
}
</script>
