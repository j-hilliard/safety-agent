export const apps = {
    billingPacketRequestSystem: {
        baseSlug: 'billing-packet-request-system',
        name: 'Billing Packet Request System',
        description: 'Create and manage billing packet requests',
        icon: 'pi pi-envelope',
        menu: {
            user: [{ label: 'Dashboard', icon: 'pi pi-fw pi-chart-line', to: 'dashboard' }],
            admin: [],
        },
    } as App,
    strongholdBizAppsSuite: {
        baseSlug: '',
        name: 'Stronghold BizApps Suite',
        description: 'Access your apps quickly and easily',
        icon: 'pi pi-th-large',
        menu: {
            user: [{ label: 'Dashboard', icon: 'pi pi-fw pi-th-large', to: 'dashboard' }],
            admin: [],
        },
    } as App,
    projectManagementSystem: {
        baseSlug: 'project-management-system',
        name: 'Project Management System',
        description: 'Manage and track your projects efficiently',
        icon: 'pi pi-briefcase',
        menu: {
            user: [{ label: 'Dashboard', icon: 'pi pi-fw pi-th-large', to: 'dashboard' }],
            admin: [],
        },
    } as App,
    incidentManagement: {
        baseSlug: 'incident-management',
        name: 'Incident Management',
        description: 'Create and manage incident reports',
        icon: 'pi pi-exclamation-triangle',
        menu: {
            user: [
                { label: 'Incidents', icon: 'pi pi-fw pi-list', to: '/incident-management/incidents' },
                { label: 'New Incident', icon: 'pi pi-fw pi-plus', to: '/incident-management/incidents/new' },
                { label: 'Investigations', icon: 'pi pi-fw pi-search', to: '/incident-management/investigations' },
            ],
            admin: [],
        },
    } as App,
};