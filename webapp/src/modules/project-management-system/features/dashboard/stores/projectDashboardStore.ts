// Project Management System Dashboard Store (Pinia)
// This store manages all state and actions for the dashboard feature.
import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { useToast } from 'primevue/usetoast';
import useVuelidate from '@vuelidate/core';
import { required } from '@vuelidate/validators';

export const useProjectDashboardStore = defineStore('projectDashboard', () => {
    // Mock project data
    const projects = ref([
        { id: 1, name: 'Website Redesign', status: 'Active', owner: 'Alice', startDate: '2024-05-01', active: true },
        { id: 2, name: 'Mobile App Launch', status: 'Completed', owner: 'Bob', startDate: '2023-11-15', active: true },
        { id: 3, name: 'Internal Tool', status: 'On Hold', owner: 'Carol', startDate: '2024-01-20', active: false },
        { id: 4, name: 'Marketing Campaign', status: 'Active', owner: 'Dave', startDate: '2024-06-10', active: true },
    ]);

    const search = ref('');
    const statusFilter = ref('');
    const statusOptions = [
        { label: 'All Statuses', value: '' },
        { label: 'Active', value: 'Active' },
        { label: 'Completed', value: 'Completed' },
        { label: 'On Hold', value: 'On Hold' },
    ];

    const filteredProjects = computed(() => {
        return projects.value.filter(project => {
            const matchesSearch =
                project.name.toLowerCase().includes(search.value.toLowerCase()) ||
                project.owner.toLowerCase().includes(search.value.toLowerCase());
            const matchesStatus = !statusFilter.value || project.status === statusFilter.value;
            return matchesSearch && matchesStatus;
        });
    });

    // Dialog state
    const projectDialogVisible = ref(false);
    const deleteDialogVisible = ref(false);
    const toggleActiveDialogVisible = ref(false);
    const dialogMode = ref<'create' | 'edit'>('create');
    const projectForm = ref({ id: 0, name: '', status: '', owner: '', startDate: '', active: true });
    const projectToDelete = ref<any>(null);
    const toggleActiveTarget = ref<any>(null);

    // Vuelidate validation instance
    const v$ = useVuelidate({
        name: { required },
        status: { required },
        owner: { required },
        startDate: { required }
    }, projectForm);

    const isFormValid = computed(() => {
        return projectForm.value.name && projectForm.value.status && projectForm.value.owner && projectForm.value.startDate;
    });

    const toggleActiveDialogHeader = computed(() => {
        if (!toggleActiveTarget.value) return '';
        return toggleActiveTarget.value.active ? 'Confirm Disable' : 'Confirm Activate';
    });

    const toast = useToast();

    function openCreateDialog() {
        dialogMode.value = 'create';
        Object.assign(projectForm.value, { id: 0, name: '', status: '', owner: '', startDate: '', active: true });
        v$.value.$reset();
        projectDialogVisible.value = true;
    }

    function openEditDialog(project: any) {
        dialogMode.value = 'edit';
        Object.assign(projectForm.value, project);
        v$.value.$reset();
        projectDialogVisible.value = true;
    }

    function viewProject(project: any) {
        openEditDialog(project);
    }

    function closeDialog() {
        projectDialogVisible.value = false;
        deleteDialogVisible.value = false;
        toggleActiveDialogVisible.value = false;
    }

    function saveProject() {
        if (!isFormValid.value) return;
        if (dialogMode.value === 'create') {
            const newId = Math.max(...projects.value.map(p => p.id)) + 1;
            projects.value.push({ ...projectForm.value, id: newId });
            toast.add({ severity: 'success', summary: 'Project Created', detail: 'The project was created successfully.', life: 3000 });
        } else if (dialogMode.value === 'edit') {
            const idx = projects.value.findIndex(p => p.id === projectForm.value.id);
            if (idx !== -1) projects.value[idx] = { ...projectForm.value };
            toast.add({ severity: 'success', summary: 'Project Updated', detail: 'The project was updated successfully.', life: 3000 });
        }
        projectDialogVisible.value = false;
    }

    function confirmDelete(project: any) {
        projectToDelete.value = project;
        deleteDialogVisible.value = true;
    }

    function deleteProject() {
        projects.value = projects.value.filter(p => p.id !== projectToDelete.value.id);
        deleteDialogVisible.value = false;
        toast.add({ severity: 'success', summary: 'Project Deleted', detail: 'The project was deleted successfully.', life: 3000 });
        projectToDelete.value = null;
    }

    function confirmToggleActive(project: any) {
        toggleActiveTarget.value = project;
        toggleActiveDialogVisible.value = true;
    }

    function toggleActiveConfirmed() {
        if (toggleActiveTarget.value) {
            toggleActiveTarget.value.active = !toggleActiveTarget.value.active;
            toast.add({
                severity: 'success',
                summary: toggleActiveTarget.value.active ? 'Project Activated' : 'Project Disabled',
                detail: `The project was ${toggleActiveTarget.value.active ? 'activated' : 'disabled'} successfully.`,
                life: 3000
            });
        }
        toggleActiveDialogVisible.value = false;
        toggleActiveTarget.value = null;
    }

    return {
        projects,
        search,
        statusFilter,
        statusOptions,
        filteredProjects,
        projectDialogVisible,
        deleteDialogVisible,
        toggleActiveDialogVisible,
        dialogMode,
        projectForm,
        projectToDelete,
        toggleActiveTarget,
        isFormValid,
        toggleActiveDialogHeader,
        openCreateDialog,
        openEditDialog,
        viewProject,
        closeDialog,
        saveProject,
        confirmDelete,
        deleteProject,
        confirmToggleActive,
        toggleActiveConfirmed,
        v$,
    };
});
