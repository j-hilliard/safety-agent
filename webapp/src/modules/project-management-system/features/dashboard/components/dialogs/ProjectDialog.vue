<template>
  <BaseDialog v-model:visible="store.projectDialogVisible" :header="store.dialogMode === 'create' ? 'Create Project' : 'Edit Project'">
    <BaseFormField label="Name" :errors="store.v$.name.$errors">
      <InputText v-model="store.projectForm.name" required class="w-full" :class="{ '!border-2 !border-red-500': store.v$.name.$error }" />
    </BaseFormField>
    <BaseFormField label="Status" :errors="store.v$.status.$errors">
      <Dropdown v-model="store.projectForm.status" :options="store.statusOptions" optionLabel="label" optionValue="value" required class="w-full" :class="{ '!border-2 !border-red-500': store.v$.status.$error }" />
    </BaseFormField>
    <BaseFormField label="Owner" :errors="store.v$.owner.$errors">
      <InputText v-model="store.projectForm.owner" required class="w-full" :class="{ '!border-2 !border-red-500': store.v$.owner.$error }" />
    </BaseFormField>
    <BaseFormField label="Start Date" :errors="store.v$.startDate.$errors">
      <Calendar v-model="store.projectForm.startDate" required showIcon class="w-full" :class="{ '!border-2 !border-red-500': store.v$.startDate.$error }" />
    </BaseFormField>
    <template #footer>
      <BaseButtonCancel @click="store.closeDialog" />
      <BaseButtonCreate v-if="store.dialogMode === 'create'" @click="handleSave" />
      <BaseButtonUpdate v-else-if="store.dialogMode === 'edit'" @click="handleSave" />
      <BaseButtonSave v-else @click="handleSave" />
    </template>
  </BaseDialog>
</template>

<script setup lang="ts">
import { useProjectDashboardStore } from '@/modules/project-management-system/features/dashboard/stores/projectDashboardStore.ts';
import BaseDialog from '@/components/dialogs/BaseDialog.vue';
import BaseFormField from '@/components/forms/BaseFormField.vue';
import BaseButtonCancel from '@/components/buttons/BaseButtonCancel.vue';
import BaseButtonCreate from '@/components/buttons/BaseButtonCreate.vue';
import BaseButtonUpdate from '@/components/buttons/BaseButtonUpdate.vue';
import BaseButtonSave from '@/components/buttons/BaseButtonSave.vue';

const store = useProjectDashboardStore();

function handleSave() {
  store.v$.$touch();
  if (!store.v$.$invalid) {
    store.saveProject();
  }
}
</script> 