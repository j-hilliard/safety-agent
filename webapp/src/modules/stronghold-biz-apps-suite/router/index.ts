import { apps } from '@/apps.ts';
import { getErrorRoutes } from '@/router/errorRoutes.ts';
import { dashboardRoutes } from '@/modules/stronghold-biz-apps-suite/features/dashboard/router';

export const strongholdBizAppsSuiteRoutes = [
    { path: '', redirect: `${apps.strongholdBizAppsSuite.baseSlug}/dashboard` },
    ...dashboardRoutes,
    ...getErrorRoutes(apps.strongholdBizAppsSuite),
];