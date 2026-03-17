<template>
    <div class="layout-topbar app-shell-topbar">
        <div class="layout-topbar-wrapper">
            <div class="layout-topbar-left">
                <router-link to="/">
                    <div class="layout-topbar-logo">
                        <img id="app-logo" :src="logo" alt="poseidon-layout" />
                    </div>
                </router-link>
            </div>
            <div class="layout-topbar-right">
                <a href="#" @click="$emit('menu-button-click', $event)" class="menu-button app-shell-menu-button">
                    <i class="pi pi-bars" />
                </a>
                <Breadcrumb :model="breadcrumbItems" class="ml-4" />
                <ul class="layout-topbar-actions">
                    <li class="topbar-item user-profile"
                        :class="{ 'active-topmenuitem': activeTopbarItem === 'profile' }">
                        <a
                            href="#"
                            class="text-white flex items-center justify-center"
                            @click="onTopbarItemClick($event, 'profile')"
                        >
                            <img
                                v-if="userStore.userPhoto"
                                alt="demo"
                                :src="userStore.userPhoto"
                                class="profile-image round-image"
                            />
                            <div v-else class="w-10 h-10 rounded-full app-shell-avatar flex items-center justify-center">
                                <i class="pi pi-user text-slate-300" />
                            </div>
                            <div class="profile-info app-shell-topbar-text">
                                <h6>{{ userStore.userFullName }}</h6>
                                <span v-if="userStore.userTitle">{{ userStore.userTitle }}</span>
                            </div>
                        </a>
                        <ul class="fadeInDown">
                            <li class="layout-submenu-header">
                                <img
                                    v-if="userStore.userPhoto"
                                    alt="demo"
                                    :src="userStore.userPhoto"
                                    class="profile-image, round-image"
                                />
                                <div v-else
                                     class="w-10 h-10 rounded-full app-shell-avatar flex items-center justify-center">
                                    <i class="pi pi-user text-slate-300" />
                                </div>
                                <div class="profile-info">
                                    <h6>{{ userStore.userFullName }}</h6>
                                    <span v-if="userStore.userTitle">{{ userStore.userTitle }}</span>
                                </div>
                            </li>
                            <li role="menuitem">
                                <a href="#" @click="userStore.logoutUser">
                                    <i class="pi pi-power-off" />
                                    <h6>Logout</h6>
                                </a>
                            </li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import logo from '@/assets/images/header-logo.svg';
import { useUserStore } from '@/stores/userStore.ts';

defineProps<{ activeTopbarItem: string; }>();

const emit = defineEmits([
    'menu-button-click',
    'topbar-item-click',
]);

const route = useRoute();
const userStore = useUserStore();

const breadcrumbItems = computed(() => {
    const routeBreadcrumbItems = route.meta?.breadcrumbItems;

    if (routeBreadcrumbItems instanceof Function) {
        return routeBreadcrumbItems() || [];
    }

    return [];
});

function onTopbarItemClick(event: Event, item: string) {
    emit('topbar-item-click', { originalEvent: event, item });
    event.preventDefault();
}
</script>

<style scoped>
.round-image {
    width: 100px;
    height: 100px;
    border-radius: 50%;
}
</style>
