type AppRoute = {
    to: string;
    icon: string;
    label: string;
};

type App = {
    baseSlug: string;
    name: string;
    description: string;
    icon: string;
    menu: { user: Array<AppRoute>; admin: Array<AppRoute> };
};

type Breadcrumb = {
    to?: string;
    icon?: string;
    label: string;
};

type RouteMeta = {
    title?: string;
    breadcrumbItems?: () => Breadcrumb[];
};