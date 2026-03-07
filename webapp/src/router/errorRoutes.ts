function getRouteMeta(app: App, route: string): RouteMeta {
    return {
        title: route,
        breadcrumbItems: () => {
            return [{ label: app.name }, { label: route }] as Breadcrumb[];
        },
    };
}

export function getErrorRoutes(app: App) {
    const baseSlug = app.baseSlug ? `/${app.baseSlug}` : '';
    const baseName = app.baseSlug ? `${app.baseSlug}-` : '';

    return [
        {
            path: 'forbidden',
            name: `${baseName}forbidden`,
            component: () => import('@/views/errors/ErrorForbiddenView.vue'),
            meta: getRouteMeta(app, '403 Forbidden'),
        },
        {
            path: 'unauthorized',
            name: `${baseName}unauthorized`,
            component: () => import('@/views/errors/ErrorUnauthorizedView.vue'),
            meta: getRouteMeta(app, '401 Unauthorized'),
        },
        {
            path: 'internal-server-error',
            name: `${baseName}internal-server-error`,
            component: () => import('@/views/errors/ErrorInternalServerView.vue'),
            meta: getRouteMeta(app, '500 Internal Server Error'),
        },
        {
            path: 'page-not-found',
            name: `${baseName}page-not-found`,
            component: () => import('@/views/errors/ErrorPageNotFoundView.vue'),
            meta: getRouteMeta(app, '404 Page Not Found'),
        },
        {
            path: `${baseSlug}/:pathMatch(.*)*`,
            name: `${baseName}page-not-found-redirect`,
            redirect: `${baseSlug}/page-not-found`,
        },
    ];
}
