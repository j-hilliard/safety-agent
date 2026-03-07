import { computed } from 'vue';
import { apps } from '@/apps.ts';
import { defineStore } from 'pinia';
import { useRoute } from 'vue-router';
import { useUserStore } from '@/stores/userStore.ts';

export const useAppStore = defineStore('app', () => {
    const route = useRoute();
    const userStore = useUserStore();

    const currentApp = computed(() => {
        for (const app in apps) {
            if (apps[app].baseSlug && route.fullPath.startsWith(`/${apps[app].baseSlug}`)) {
                return apps[app];
            }
        }

        return apps.strongholdBizAppsSuite;
    });

    const menu = computed(() => {
        const routes = [{ label: currentApp.value.name, items: currentApp.value.menu.user }];

        if (userStore.isAdmin && currentApp.value.menu.admin.length) {
            routes.push({ label: 'Administration', items: currentApp.value.menu.admin });
        }

        return routes;
    });

    return {
        menu,
        currentApp,
    };
});
