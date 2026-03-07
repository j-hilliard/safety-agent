<template>
    <div class="layout-menu-container bg-slate-900">
        <TheSubmenu
            root
            :items="model"
            :menuMode="menuMode"
            parentMenuItemActive
            :menuActive="active"
            :mobileMenuActive="mobileMenuActive"
            class="layout-menu"
            @menuitem-click="onMenuItemClick"
            @root-menuitem-click="onRootMenuItemClick"
        />
    </div>
</template>

<script setup lang="ts">
import TheSubmenu from '@/components/layout/TheSubmenu.vue';

defineProps({
    model: {
        type: Array,
        required: true,
    },
    active: {
        type: Boolean,
        required: true,
    },
    menuMode: {
        type: String,
        required: true,
    },
    mobileMenuActive: {
        type: Boolean,
        required: true,
    },
});

const emit = defineEmits(['menu-click', 'menuitem-click', 'root-menuitem-click']);

function onMenuItemClick(event: Event) {
    emit('menuitem-click', event);
}

function onRootMenuItemClick(event: Event) {
    emit('root-menuitem-click', event);
}
</script>
