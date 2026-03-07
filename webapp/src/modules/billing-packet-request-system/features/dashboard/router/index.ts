import { apps } from '@/apps.ts';

export const dashboardRoutes = [
    {
        path: 'dashboard',
        name: `${apps.billingPacketRequestSystem.baseSlug}-dashboard`,
        component: () => import('@/modules/billing-packet-request-system/features/dashboard/views/DashboardView.vue'),
        meta: {
            title: 'Dashboard',
            breadcrumbItems: (): Breadcrumb[] => {
                return [{ label: apps.billingPacketRequestSystem.name }, { label: 'Dashboard' }];
            },
        } as RouteMeta,
    },
];