import { apps } from '@/apps.ts';

export const incidentListRoutes = [
    {
        path: 'incidents',
        name: `${apps.incidentManagement.baseSlug}-incidents`,
        component: () => import('@/modules/incident-management/features/incident-list/views/IncidentListView.vue'),
        meta: {
            title: 'Incidents',
            breadcrumbItems: () => [
                { label: apps.incidentManagement.name },
                { label: 'Incidents' },
            ],
        },
    },
];
