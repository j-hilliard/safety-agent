<template>
    <div class="flex items-end justify-between mt-8 mb-4">
        <h2 class="text-xl font-bold text-white">{{ title }}</h2>
        <slot />
    </div>
</template>

<script setup lang="ts">
withDefaults(defineProps<{
    title?: string;
}>(), {
    title: '',
});
</script>
