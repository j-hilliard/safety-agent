<template>
    <div class="flex flex-col md:flex-row md:items-center md:justify-between mb-4 gap-4">
        <div class="flex items-center gap-4">
            <i v-if="icon" :class="[icon, '!text-4xl text-blue-400']" />
            <div>
                <h1 class="text-2xl font-bold text-white">{{ title }}</h1>
                <p class="text-slate-400">{{ subtitle }}</p>
            </div>
        </div>
        <div class="flex flex-wrap justify-end items-center gap-4 md:mt-0 mt-2">
            <slot />
        </div>
    </div>
</template>

<script setup lang="ts">
withDefaults(defineProps<{
    icon?: string;
    title?: string;
    subtitle?: string;
}>(), {
    icon: '',
    title: '',
    subtitle: '',
});
</script>
