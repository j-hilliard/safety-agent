import { apps } from '@/apps.ts';

export const incidentFormRoutes = [
    {
        path: 'incidents/new',
        name: `${apps.incidentManagement.baseSlug}-incident-create`,
        component: () => import('@/modules/incident-management/features/incident-form/views/IncidentFormView.vue'),
        meta: {
            title: 'New Incident',
            breadcrumbItems: () => [
                { label: apps.incidentManagement.name },
                { label: 'Incidents', to: `/${apps.incidentManagement.baseSlug}/incidents` },
                { label: 'New' },
            ],
        },
    },
    {
        path: 'incidents/:id',
        name: `${apps.incidentManagement.baseSlug}-incident-edit`,
        component: () => import('@/modules/incident-management/features/incident-form/views/IncidentFormView.vue'),
        meta: {
            title: 'Edit Incident',
            breadcrumbItems: () => [
                { label: apps.incidentManagement.name },
                { label: 'Incidents', to: `/${apps.incidentManagement.baseSlug}/incidents` },
                { label: 'Edit' },
            ],
        },
    },
];
