<template>
  <BaseDialog v-model:visible="store.deleteDialogVisible" header="Confirm Delete">
    <div>Are you sure you want to delete <b>{{ store.projectToDelete?.name }}</b>?</div>
    <template #footer>
      <BaseButtonCancel @click="store.closeDialog" />
      <BaseButtonDelete @click="store.deleteProject" />
    </template>
  </BaseDialog>
</template>

<script setup lang="ts">
import { useProjectDashboardStore } from '@/modules/project-management-system/features/dashboard/stores/projectDashboardStore.ts';
import BaseDialog from '@/components/dialogs/BaseDialog.vue';
import BaseButtonCancel from '@/components/buttons/BaseButtonCancel.vue';
import BaseButtonDelete from '@/components/buttons/BaseButtonDelete.vue';
const store = useProjectDashboardStore();
</script> 