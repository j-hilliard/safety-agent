<template>
    <div>
        <div class="flex items-center justify-between mb-2">
            <h3 class="text-base font-semibold text-white">Corrective / Preventive Actions</h3>
            <Button label="Add Action" icon="pi pi-plus" size="small" @click="store.addAction()" />
        </div>

        <DataTable :value="store.form.actions ?? []" emptyMessage="No actions added.">
            <Column header="Type" style="width: 160px;">
                <template #body="{ data }">
                    <Dropdown
                        v-model="data.actionType"
                        :options="actionTypeOptions"
                        placeholder="Type"
                        class="w-full"
                        size="small"
                        :showClear="true"
                    />
                </template>
            </Column>
            <Column header="Description">
                <template #body="{ data }">
                    <InputText v-model="data.actionDescription" class="w-full" placeholder="Describe the action" size="small" />
                </template>
            </Column>
            <Column header="Assigned To" style="width: 160px;">
                <template #body="{ data }">
                    <InputText v-model="data.assignedTo" class="w-full" placeholder="Name" size="small" />
                </template>
            </Column>
            <Column header="Due Date" style="width: 140px;">
                <template #body="{ data }">
                    <Calendar v-model="data.dueDate" showIcon class="w-full" dateFormat="mm/dd/yy" size="small" />
                </template>
            </Column>
            <Column header="Status" style="width: 140px;">
                <template #body="{ data }">
                    <Dropdown
                        v-model="data.status"
                        :options="actionStatusOptions"
                        placeholder="Status"
                        class="w-full"
                        size="small"
                        :showClear="true"
                    />
                </template>
            </Column>
            <Column header="" style="width: 60px;">
                <template #body="{ index }">
                    <BaseButtonIconDelete @click="store.removeAction(index)" />
                </template>
            </Column>
        </DataTable>
    </div>
</template>

<script setup lang="ts">
import { useIncidentStore } from '@/modules/incident-management/stores/incidentStore';
import BaseButtonIconDelete from '@/components/buttons/BaseButtonIconDelete.vue';

const store = useIncidentStore();

const actionTypeOptions = ['Corrective', 'Preventive', 'Immediate'];
const actionStatusOptions = ['Open', 'In Progress', 'Closed'];
</script>
