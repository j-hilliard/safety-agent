export const refTableMaintenanceRoutes = [
    {
        path: 'ref-tables',
        name: 'incident-management-ref-tables',
        component: () => import('../views/RefTableMaintenanceView.vue'),
        meta: {
            title: 'Reference Table Maintenance',
            breadcrumbItems: () => [
                { label: 'Incident Management' },
                { label: 'Reference Tables' },
            ],
        },
    },
];
