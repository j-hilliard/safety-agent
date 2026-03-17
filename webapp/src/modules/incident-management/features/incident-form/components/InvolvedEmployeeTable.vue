<template>
    <div>
        <div class="flex items-center justify-between mb-2">
            <h3 class="text-base font-semibold text-white">Involved Employees</h3>
            <Button label="Add Employee" icon="pi pi-plus" size="small" @click="store.addEmployee()" />
        </div>

        <DataTable :value="store.form.employeesInvolved ?? []" emptyMessage="No employees added.">
            <Column header="Employee ID" style="width: 140px;">
                <template #body="{ data }">
                    <InputText v-model="data.employeeIdentifier" class="w-full" placeholder="ID" size="small" />
                </template>
            </Column>
            <Column header="Employee Name">
                <template #body="{ data }">
                    <InputText v-model="data.employeeName" class="w-full" placeholder="Full name" size="small" />
                </template>
            </Column>
            <Column header="Injury Type" style="width: 160px;">
                <template #body="{ data }">
                    <Dropdown
                        v-model="data.injuryTypeCode"
                        :options="injuryTypeOptions"
                        optionLabel="name"
                        optionValue="code"
                        placeholder="Type"
                        class="w-full"
                        size="small"
                        :showClear="true"
                    />
                </template>
            </Column>
            <Column header="Hours Worked" style="width: 120px;">
                <template #body="{ data }">
                    <InputNumber v-model="data.hoursWorked" class="w-full" :minFractionDigits="1" :maxFractionDigits="2" size="small" />
                </template>
            </Column>
            <Column header="Recordable" style="width: 100px;">
                <template #body="{ data }">
                    <Checkbox v-model="data.recordable" :binary="true" />
                </template>
            </Column>
            <Column header="" style="width: 60px;">
                <template #body="{ index }">
                    <BaseButtonIconDelete @click="store.removeEmployee(index)" />
                </template>
            </Column>
        </DataTable>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useIncidentStore } from '@/modules/incident-management/stores/incidentStore';
import { useReferenceDataStore } from '@/modules/incident-management/stores/referenceDataStore';
import BaseButtonIconDelete from '@/components/buttons/BaseButtonIconDelete.vue';

const store = useIncidentStore();
const refStore = useReferenceDataStore();

const injuryTypeOptions = computed(() => refStore.getOptionsByType('injury_type'));
</script>
