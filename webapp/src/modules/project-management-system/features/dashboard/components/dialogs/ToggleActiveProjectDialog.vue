<template>
  <BaseDialog v-model:visible="store.toggleActiveDialogVisible" :header="store.toggleActiveDialogHeader">
    <div>
      Are you sure you want to
      <b>{{ store.toggleActiveTarget?.active ? 'disable' : 'activate' }}</b>
      <b>{{ store.toggleActiveTarget?.name }}</b>?
    </div>
    <template #footer>
      <BaseButtonCancel @click="store.closeDialog" />
      <BaseButtonDisable v-if="store.toggleActiveTarget?.active" @click="store.toggleActiveConfirmed" />
      <BaseButtonActivate v-else @click="store.toggleActiveConfirmed" />
    </template>
  </BaseDialog>
</template>

<script setup lang="ts">
import { useProjectDashboardStore } from '@/modules/project-management-system/features/dashboard/stores/projectDashboardStore.ts';
import BaseDialog from '@/components/dialogs/BaseDialog.vue';
import BaseButtonCancel from '@/components/buttons/BaseButtonCancel.vue';
import BaseButtonDisable from '@/components/buttons/BaseButtonDisable.vue';
import BaseButtonActivate from '@/components/buttons/BaseButtonActivate.vue';
const store = useProjectDashboardStore();
</script> 