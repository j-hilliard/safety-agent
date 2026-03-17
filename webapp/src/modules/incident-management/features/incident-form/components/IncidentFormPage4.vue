<template>
    <Card>
        <template #title>Page 4 — Actions &amp; Leadership Review</template>
        <template #content>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">

                <!-- Health & Safety Leader -->
                <BaseFormField label="Health & Safety Leader">
                    <InputText v-model="healthSafetyLeaderDisplay" class="w-full" placeholder="Search by name or email" />
                </BaseFormField>

                <!-- Senior Ops Leader -->
                <BaseFormField label="Senior Ops Leader">
                    <InputText v-model="seniorOpsLeaderDisplay" class="w-full" placeholder="Search by name or email" />
                </BaseFormField>

                <!-- Corrective / Preventive Actions -->
                <div class="md:col-span-2">
                    <IncidentActionTable />
                </div>
            </div>
        </template>
    </Card>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useIncidentStore } from '@/modules/incident-management/stores/incidentStore';
import BaseFormField from '@/components/forms/BaseFormField.vue';
import IncidentActionTable from '@/modules/incident-management/features/incident-form/components/IncidentActionTable.vue';

const store = useIncidentStore();

// Simple text fields for leader lookup — can be wired to AD search later
const healthSafetyLeaderDisplay = ref('');
const seniorOpsLeaderDisplay = ref('');
</script>
