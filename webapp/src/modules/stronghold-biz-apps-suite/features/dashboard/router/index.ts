import { apps } from '@/apps.ts';

export const dashboardRoutes = [
    {
        path: 'dashboard',
        name: 'dashboard',
        component: () => import('@/modules/stronghold-biz-apps-suite/features/dashboard/views/DashboardView.vue'),
        meta: {
            title: 'Dashboard',
            breadcrumbItems: (): Breadcrumb[] => {
                return [{ label: apps.strongholdBizAppsSuite.name }, { label: 'Dashboard' }];
            },
        } as RouteMeta,
    },
];