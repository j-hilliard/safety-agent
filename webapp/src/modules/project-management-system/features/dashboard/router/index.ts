import { apps } from '@/apps.ts';

export const dashboardRoutes = [
    {
        path: 'dashboard',
        name: `${apps.projectManagementSystem.baseSlug}-dashboard`,
        component: () => import('@/modules/project-management-system/features/dashboard/views/DashboardView.vue'),
        meta: {
            title: 'Dashboard',
            breadcrumbItems: () => [
                { label: apps.projectManagementSystem.name },
                { label: 'Dashboard' },
            ],
        },
    },
]; 