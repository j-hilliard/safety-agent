<template>
    <ul v-if="items" role="menu">
        <template v-for="(item, index) of items">
            <li
                v-if="visible(item) && !item.separator && !item.isHidden" :key="item.label || index"
                :class="[{ 'layout-root-menuitem': root, 'active-menuitem': activeIndex === index && !item.disabled }]"
                role="menuitem"
            >
                <div v-if="root && menuMode !== 'horizontal' && item?.visible !== false">
                    <span class="layout-menuitem-text">{{ item.label }}</span>
                </div>
                <router-link
                    v-if="item.to"
                    v-ripple
                    exact
                    :to="item.to"
                    :style="item.style"
                    :target="item.target"
                    exact-active-class="active-route"
                    :class="[item.class, 'p-ripple', { 'p-disabled': item.disabled }, { 'active-route': isActiveRoute(item) }]"
                    @mouseenter="onMenuItemMouseEnter(index)"
                    @click="onMenuItemClick($event, item, index)"
                >
                    <i :class="['layout-menuitem-icon', item.icon]" />
                    <span class="layout-menuitem-text">{{ item.label }}</span>
                    <i v-if="item.items" class="pi pi-fw pi-angle-down layout-submenu-toggler" />
                </router-link>
                <a
                    v-if="!item.to"
                    v-ripple
                    :style="item.style"
                    :target="item.target"
                    :href="item.url || '#'"
                    :class="[item.class, 'p-ripple', { 'p-disabled': item.disabled }]"
                    @mouseenter="onMenuItemMouseEnter(index)"
                    @click="onMenuItemClick($event, item, index)"
                >
                    <i :class="['layout-menuitem-icon', item.icon]" />
                    <span class="layout-menuitem-text">{{ item.label }}</span>
                    <i v-if="item.items" class="pi pi-fw pi-angle-down layout-submenu-toggler" />
                </a>
                <transition name="layout-menu">
                    <TheSubmenu
                        v-show="isItemActive(item, index)"
                        :menuMode="menuMode"
                        :menuActive="menuActive"
                        :items="visible(item) && item.items"
                        :parentMenuItemActive="activeIndex === index"
                        @menuitem-click="$emit('menuitem-click', $event)"
                    />
                </transition>
            </li>
        </template>
    </ul>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router';
import EventBus from '@/layout/event-bus.ts';
import { computed, onMounted, ref } from 'vue';

const props = defineProps({
    items: {
        type: Array,
        required: false,
    },
    menuMode: {
        type: String,
        required: true,
    },
    menuActive: {
        type: Boolean,
        required: true,
    },
    mobileMenuActive: {
        type: Boolean,
        required: false,
    },
    root: {
        type: Boolean,
        default: false,
    },
    parentMenuItemActive: {
        type: Boolean,
        default: false,
    },
});

const emit = defineEmits(['root-menuitem-click', 'menuitem-click']);

const activeIndex = ref<number | null>(-1);

const route = useRoute();

const isMobile = computed(() => {
    return window.innerWidth <= 1091;
});

onMounted(() => {
    EventBus.on('reset-active-index', () => {
        if (props.menuMode === 'horizontal') {
            activeIndex.value = null;
        }
    });
});

function isActiveRoute(item) {
    return route.path.startsWith(item.to) && item.to !== '/';
}

function isItemActive(item: { items?: unknown }, index: number) {
    if (!item.items) {
        return false;
    }

    if (props.root) {
        if (props.menuMode !== 'horizontal') {
            return true;
        }

        if (props.menuMode === 'horizontal' && (props.mobileMenuActive || activeIndex.value !== null)) {
            return true;
        }
    }

    return activeIndex.value === index;
}

function onMenuItemClick(event, item, index) {
    if (item.disabled) {
        event.preventDefault();
        return;
    }

    if (item.command) {
        item.command({ originalEvent: event, item: item });
        event.preventDefault();
    }

    if (item.items) {
        event.preventDefault();
    }

    if (props.root) {
        emit('root-menuitem-click', {
            originalEvent: event,
        });
    }

    if (item.items) {
        activeIndex.value = index === activeIndex.value ? null : index;
    } else if (props.menuMode !== 'static') {
        const ink = getInk(event.currentTarget);

        if (ink) {
            removeClass(ink, 'p-ink-active');
        }
    }

    emit('menuitem-click', {
        item: item,
        originalEvent: event,
    });
}

function onMenuItemMouseEnter(index) {
    if (props.root && props.menuActive && props.menuMode === 'horizontal' && !isMobile.value) {
        activeIndex.value = index;
    }
}

function visible(item) {
    return typeof item.visible === 'function' ? item.visible() : item?.visible !== false;
}

function getInk(el) {
    for (let i = 0; i < el.children.length; i++) {
        if (typeof el.children[i].className === 'string' && el.children[i].className.indexOf('p-ink') !== -1) {
            return el.children[i];
        }
    }

    return null;
}

function removeClass(element, className) {
    if (element.classList) element.classList.remove(className);
    else element.className = element.className.replace(new RegExp('(^|\\b)' + className.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
}
</script>
