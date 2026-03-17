import { apps } from '@/apps.ts';
import { getErrorRoutes } from '@/router/errorRoutes.ts';
import { incidentListRoutes } from '@/modules/incident-management/features/incident-list/router';
import { incidentFormRoutes } from '@/modules/incident-management/features/incident-form/router';
import { refTableMaintenanceRoutes } from '@/modules/incident-management/features/ref-table-maintenance/router';

export const incidentManagementRoutes = [
    { path: '', redirect: `${apps.incidentManagement.baseSlug}/incidents` },
    ...incidentListRoutes,
    ...incidentFormRoutes,
    ...refTableMaintenanceRoutes,
    {
        path: 'investigations',
        name: 'incident-management-investigations',
        component: () => import('@/modules/incident-management/features/investigation/views/InvestigationListView.vue'),
        meta: {
            title: 'Investigations',
            breadcrumbItems: () => [
                { label: 'Incident Management' },
                { label: 'Investigations' },
            ],
        },
    },
    ...getErrorRoutes(apps.incidentManagement),
];
