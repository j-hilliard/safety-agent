import { apps } from '@/apps.ts';
import { getErrorRoutes } from '@/router/errorRoutes.ts';
import { dashboardRoutes } from '@/modules/billing-packet-request-system/features/dashboard/router';

export const billingPacketRoutes = [
    { path: '', redirect: `${apps.billingPacketRequestSystem.baseSlug}/dashboard` },
    ...dashboardRoutes,
    ...getErrorRoutes(apps.billingPacketRequestSystem),
];