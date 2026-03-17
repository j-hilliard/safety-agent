<template>
    <div>
        <BasePageHeader
            icon="pi pi-exclamation-triangle"
            title="Incidents"
            subtitle="View and manage incident reports"
        >
            <BaseButtonCreate label="New Incident" @click="router.push(`/${apps.incidentManagement.baseSlug}/incidents/new`)" />
        </BasePageHeader>

        <IncidentListTable />
    </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { apps } from '@/apps.ts';
import { useIncidentStore } from '@/modules/incident-management/stores/incidentStore';
import { useReferenceDataStore } from '@/modules/incident-management/stores/referenceDataStore';
import BasePageHeader from '@/components/layout/BasePageHeader.vue';
import BaseButtonCreate from '@/components/buttons/BaseButtonCreate.vue';
import IncidentListTable from '@/modules/incident-management/features/incident-list/components/IncidentListTable.vue';

const router = useRouter();
const store = useIncidentStore();
const refStore = useReferenceDataStore();

onMounted(async () => {
    await refStore.loadAll();
    await store.loadList();
});
</script>
