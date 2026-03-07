import { apps } from '@/apps.ts';
import { getErrorRoutes } from '@/router/errorRoutes.ts';
import { dashboardRoutes } from '@/modules/project-management-system/features/dashboard/router';
 
export const projectManagementSystemRoutes = [
    { path: '', redirect: `${apps.projectManagementSystem.baseSlug}/dashboard` },
    ...dashboardRoutes,
    ...getErrorRoutes(apps.projectManagementSystem),
]; 