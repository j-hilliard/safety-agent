import { apps } from '@/apps.ts';
import { useAppStore } from '@/stores/appStore.ts';
import { createRouter, createWebHistory } from 'vue-router';
import { billingPacketRoutes } from '@/modules/billing-packet-request-system/router';
import { strongholdBizAppsSuiteRoutes } from '@/modules/stronghold-biz-apps-suite/router';
import { projectManagementSystemRoutes } from '@/modules/project-management-system/router';
import { incidentManagementRoutes } from '@/modules/incident-management/router';
import NProgress from 'nprogress';
import 'nprogress/nprogress.css';

const routes = [
    {
        path: `/${apps.billingPacketRequestSystem.baseSlug}`,
        component: () => import('@/layout/AppLayout.vue'),
        children: billingPacketRoutes,
    },
    {
        path: `/${apps.strongholdBizAppsSuite.baseSlug}`,
        component: () => import('@/layout/AppLayout.vue'),
        children: strongholdBizAppsSuiteRoutes,
    },
    {
        path: `/${apps.projectManagementSystem.baseSlug}`,
        component: () => import('@/layout/AppLayout.vue'),
        children: projectManagementSystemRoutes,
    },
    {
        path: `/${apps.incidentManagement.baseSlug}`,
        component: () => import('@/layout/AppLayout.vue'),
        children: incidentManagementRoutes,
    },
    {
        path: '/authentication/login-callback',
        name: 'authentication-login-callback',
        component: () => import('@/views/auth/AuthRedirectView.vue'),
    },
];

const router = createRouter({
    routes,
    history: createWebHistory(),
    scrollBehavior() {
        return { left: 0, top: 0 };
    },
});

router.beforeEach((to, from, next) => {
    NProgress.start();
    if (to.path !== '/' && to.path.endsWith('/')) {
        const trimmedPath = to.path.slice(0, -1);
        next({ path: trimmedPath, query: to.query, hash: to.hash });
    } else {
        next();
    }
});

router.afterEach((to) => {
    NProgress.done();
    const appStore = useAppStore();
    const routeTitle = to.meta.title ? `${to.meta.title} - ` : '';
    const appName = appStore.currentApp.name || 'The Stronghold Companies';
    document.title = routeTitle + appName;
});

router.onError(() => {
    NProgress.done();
});

export default router;
