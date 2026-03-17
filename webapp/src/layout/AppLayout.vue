<template>
    <div :class="containerClass" @click="onDocumentClick">
        <PrimeVueToast />
        <TheTopBar
            :topbarTheme="topbarTheme"
            :activeTopbarItem="activeTopBarItem"
            @topbar-item-click="onTopbarItemClick"
            @menu-button-click="onMenuButtonClick($event)"
        />
        <div class="menu-wrapper" @click="onMenuClick($event)">
            <TheMenu
                :model="appStore.menu"
                :active="menuActive"
                :menuMode="menuMode"
                :mobileMenuActive="staticMenuMobileActive"
                @menu-click="onMenuClick"
                @menuitem-click="onMenuItemClick"
            />
        </div>
        <div class="layout-main app-shell-main">
            <div class="layout-content p-12">
                <router-view v-slot="{ Component }">
                    <Transition name="fade" mode="out-in">
                        <component :is="Component" :key="route.fullPath" />
                    </Transition>
                </router-view>
            </div>
        </div>
        <div v-if="staticMenuMobileActive" class="layout-mask modal-in" @click="staticMenuMobileActive = false"></div>
    </div>
</template>

<script setup lang="ts">
import PrimeVueToast from 'primevue/toast';
import { useToast } from 'primevue/usetoast';
import EventBus from '@/layout/event-bus.ts';
import TheMenu from '@/components/layout/TheMenu.vue';
import TheTopBar from '@/components/layout/TheTopBar.vue';
import { useAppStore } from '@/stores/appStore.ts';
import { ref, computed, onBeforeMount, onUnmounted, watch } from 'vue';
import { useRoute } from 'vue-router';

const menuTheme = ref('dim');
const menuClick = ref(false);
const menuActive = ref(false);
const searchClick = ref(false);
const configClick = ref(false);
const menuMode = ref('static');
const searchActive = ref(false);
const topbarTheme = ref('light');
const activeTopBarItem = ref('');
const rightPanelClick = ref(false);
const rightPanelActive = ref(false);
const topbarMenuActive = ref(false);
const overlayMenuActive = ref(false);
const staticMenuMobileActive = ref(false);
const staticMenuDesktopInactive = ref(false);

const appStore = useAppStore();
const toast = useToast();
const route = useRoute();

const isMobile = computed(() => {
    return window.innerWidth <= 640;
});

const isDesktop = computed(() => {
    return window.innerWidth > 992;
});

const isOverlay = computed(() => {
    return menuMode.value === 'overlay';
});

const containerClass = computed(() => [
    'layout-wrapper',
    {
        'layout-static': menuMode.value === 'static',
        'layout-overlay': menuMode.value === 'overlay',
        'layout-overlay-active': overlayMenuActive.value,
        'layout-rightpanel-active': rightPanelActive.value,
        'layout-horizontal': menuMode.value === 'horizontal',
        'layout-mobile-active': staticMenuMobileActive.value,
        'layout-static-active': !staticMenuDesktopInactive.value && menuMode.value === 'static',
    },
    `layout-menu-${menuTheme.value} layout-topbar-${topbarTheme.value}`,
]);

watch(() => appStore.currentApp, (app) => {
    toast.add({ severity: 'info', summary: 'App Change', detail: `You are now working in ${app.name}`, life: 3000 });
});

onBeforeMount(() => {
    document.addEventListener('click', onDocumentClick);
});

onUnmounted(() => {
    document.removeEventListener('click', onDocumentClick);
});

function onDocumentClick() {
    if (!searchClick.value) {
        searchActive.value = false;
    }

    if (!rightPanelClick.value) {
        rightPanelActive.value = false;
    }

    if (!menuClick.value) {
        overlayMenuActive.value = false;
    }

    searchClick.value = configClick.value = menuClick.value = rightPanelClick.value = false;
}

function blockBodyScroll() {
    if (document.body.classList) {
        document.body.classList.add('blocked-scroll');
    } else {
        document.body.className += ' blocked-scroll';
    }
}

function unblockBodyScroll() {
    if (document.body.classList) {
        document.body.classList.remove('blocked-scroll');
    } else {
        document.body.className = document.body.className.replace(new RegExp('(^|\\b)' + 'blocked-scroll'.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
    }
}

function onMenuClick(event: Event) {
    menuClick.value = true;
    event.stopPropagation();
}

function onMenuButtonClick(event: Event) {
    topbarMenuActive.value = false;
    menuClick.value = true;

    if (isOverlay.value && !isMobile.value) {
        overlayMenuActive.value = !overlayMenuActive.value;
    }

    if (isDesktop.value) {
        staticMenuDesktopInactive.value = !staticMenuDesktopInactive.value;
    } else {
        staticMenuMobileActive.value = !staticMenuMobileActive.value;

        if (staticMenuMobileActive.value) {
            blockBodyScroll();
        } else {
            unblockBodyScroll();
        }
    }

    event.preventDefault();
}

function onMenuItemClick(event) {
    if (!event.item.items) {
        EventBus.emit('reset-active-index');
        overlayMenuActive.value = false;
    }

    if (isMobile.value) {
        onMenuButtonClick(event.originalEvent);
    }
}

function onTopbarItemClick(event) {
    activeTopBarItem.value = activeTopBarItem.value === event.item ? '' : event.item;
}
</script>
