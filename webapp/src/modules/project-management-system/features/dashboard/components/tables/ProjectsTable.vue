<template>
<BaseDataTable :value="store.filteredProjects" emptyMessage="No projects found.">
  <template #filters>
    <InputText
      v-model="store.search"
      placeholder="Search projects..."
      class="flex-1 w-full md:w-64"
    />
    <Dropdown
      v-model="store.statusFilter"
      :options="store.statusOptions"
      optionLabel="label"
      optionValue="value"
      placeholder="All Statuses"
      class="w-full md:w-56"
      :showClear="!!store.statusFilter"
    />
  </template>
  <Column field="name" header="Name" />
  <Column field="status" header="Status" />
  <Column field="owner" header="Owner" />
  <Column field="startDate" header="Start Date">
    <template #body="{ data }">
      {{ formatDateMMDDYYYY(data.startDate) }}
    </template>
  </Column>
  <Column header="Active">
    <template #body="{ data }">
      <Tag :value="data.active ? 'Active' : 'Disabled'" :severity="data.active ? 'success' : 'danger'" />
    </template>
  </Column>
  <Column header="Actions" style="width: 220px;">
    <template #body="{ data }">
      <BaseButtonIconView @click="store.viewProject(data)" />
      <BaseButtonIconEdit @click="store.openEditDialog(data)" />
      <BaseButtonIconDisable v-if="data.active" @click="store.confirmToggleActive(data)" />
      <BaseButtonIconActivate v-else @click="store.confirmToggleActive(data)" />
      <BaseButtonIconDelete v-if="!data.active" @click="store.confirmDelete(data)" />
    </template>
  </Column>
</BaseDataTable>
</template>

<script setup lang="ts">
import { useProjectDashboardStore } from '@/modules/project-management-system/features/dashboard/stores/projectDashboardStore';
import { formatDateMMDDYYYY } from '@/utils.ts';
import BaseButtonIconView from '@/components/buttons/BaseButtonIconView.vue';
import BaseButtonIconEdit from '@/components/buttons/BaseButtonIconEdit.vue';
import BaseButtonIconDelete from '@/components/buttons/BaseButtonIconDelete.vue';
import BaseButtonIconActivate from '@/components/buttons/BaseButtonIconActivate.vue';
import BaseButtonIconDisable from '@/components/buttons/BaseButtonIconDisable.vue';
import BaseDataTable from '@/components/tables/BaseDataTable.vue';
const store = useProjectDashboardStore();
</script> 