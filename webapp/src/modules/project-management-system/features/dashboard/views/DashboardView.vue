<template>
    <div>
        <BasePageHeader
            :title="apps.projectManagementSystem.name"
            :subtitle="apps.projectManagementSystem.description"
            :icon="apps.projectManagementSystem.icon"
        >
            <Button label="Create Project" icon="pi pi-plus" class="ml-4" @click="store.openCreateDialog" />
        </BasePageHeader>
        <ProjectsTable />
        <ProjectDialog />
        <DeleteProjectDialog />
        <ToggleActiveProjectDialog />
    </div>
</template>

<script setup lang="ts">
import { apps } from '@/apps.ts';
import ProjectsTable from '@/modules/project-management-system/features/dashboard/components/tables/ProjectsTable.vue';
import ProjectDialog from '@/modules/project-management-system/features/dashboard/components/dialogs/ProjectDialog.vue';
import DeleteProjectDialog from '@/modules/project-management-system/features/dashboard/components/dialogs/DeleteProjectDialog.vue';
import ToggleActiveProjectDialog from '@/modules/project-management-system/features/dashboard/components/dialogs/ToggleActiveProjectDialog.vue';
import { useProjectDashboardStore } from '@/modules/project-management-system/features/dashboard/stores/projectDashboardStore.ts';
import BasePageHeader from '@/components/layout/BasePageHeader.vue';
const store = useProjectDashboardStore();
</script> 