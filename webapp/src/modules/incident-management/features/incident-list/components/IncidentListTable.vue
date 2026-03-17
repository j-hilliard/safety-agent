<template>
    <BaseDataTable :value="store.incidents" :loading="store.loading" emptyMessage="No incidents found.">
        <template #filters>
            <InputText
                v-model="store.searchTerm"
                placeholder="Search incidents..."
                class="flex-1 w-full md:w-64"
                @keyup.enter="store.loadList()"
            />
            <Dropdown
                v-model="store.filterCompanyId"
                :options="refStore.companies"
                optionLabel="name"
                optionValue="id"
                placeholder="All Companies"
                class="w-full md:w-48"
                :showClear="!!store.filterCompanyId"
                @change="store.loadList()"
            />
            <Dropdown
                v-model="store.filterStatus"
                :options="statusOptions"
                optionLabel="label"
                optionValue="value"
                placeholder="All Statuses"
                class="w-full md:w-48"
                :showClear="!!store.filterStatus"
                @change="store.loadList()"
            />
            <Button icon="pi pi-search" @click="store.loadList()" />
        </template>

        <Column field="incidentNumber" header="Incident #" sortable />
        <Column field="incidentDate" header="Date" sortable>
            <template #body="{ data }">
                {{ formatDate(data.incidentDate) }}
            </template>
        </Column>
        <Column field="companyName" header="Company" sortable />
        <Column field="regionName" header="Region" sortable />
        <Column field="jobNumber" header="Job #" />
        <Column field="status" header="Status" sortable>
            <template #body="{ data }">
                <Tag :value="data.status" :severity="statusSeverity(data.status)" />
            </template>
        </Column>
        <Column field="severityActualCode" header="Severity" sortable />
        <Column header="Actions" style="width: 120px;">
            <template #body="{ data }">
                <BaseButtonIconEdit @click="router.push(`/${apps.incidentManagement.baseSlug}/incidents/${data.id}`)" />
                <BaseButtonIconDelete @click="confirmDelete(data)" />
            </template>
        </Column>
    </BaseDataTable>

    <ConfirmDialog />
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
import { useConfirm } from 'primevue/useconfirm';
import { apps } from '@/apps.ts';
import { useIncidentStore } from '@/modules/incident-management/stores/incidentStore';
import { useReferenceDataStore } from '@/modules/incident-management/stores/referenceDataStore';
import BaseDataTable from '@/components/tables/BaseDataTable.vue';
import BaseButtonIconEdit from '@/components/buttons/BaseButtonIconEdit.vue';
import BaseButtonIconDelete from '@/components/buttons/BaseButtonIconDelete.vue';
import type { IncidentReportListItem } from '@/apiclient/client';
import ConfirmDialog from 'primevue/confirmdialog';

const router = useRouter();
const store = useIncidentStore();
const refStore = useReferenceDataStore();
const confirm = useConfirm();

const statusOptions = [
    { label: 'First Report', value: 'FIRSTREPORT' },
    { label: 'Classification', value: 'CLASSIFICATION' },
    { label: 'Submitted', value: 'SUBMIT' },
    { label: 'Investigation', value: 'INVESTIGATION' },
    { label: 'Final Review', value: 'FINALREVIEW' },
    { label: 'Closed', value: 'CLOSED' },
];

function formatDate(date: Date | undefined): string {
    if (!date) return '';
    return new Date(date).toLocaleDateString('en-US', { month: '2-digit', day: '2-digit', year: 'numeric' });
}

function statusSeverity(status: string): string {
    const map: Record<string, string> = {
        FIRSTREPORT: 'info',
        CLASSIFICATION: 'warning',
        SUBMIT: 'warning',
        INVESTIGATION: 'danger',
        FINALREVIEW: 'warning',
        CLOSED: 'success',
        CLOSEOUT: 'success',
    };
    return map[status] ?? 'secondary';
}

function confirmDelete(incident: IncidentReportListItem) {
    confirm.require({
        message: `Delete incident ${incident.incidentNumber}? This cannot be undone.`,
        header: 'Confirm Delete',
        icon: 'pi pi-exclamation-triangle',
        acceptClass: 'p-button-danger',
        accept: () => store.deleteIncident(incident.id!),
    });
}
</script>
