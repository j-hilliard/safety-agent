<template>
    <div>
        <label class="block text-sm font-medium text-slate-300 mb-1">{{ label }}</label>
        <slot />
        <div v-if="errors.length">
            <small class="text-red-600" v-for="(error, index) of errors" :key="index">
                {{ error?.$message || 'Unknown error occurred' }}
            </small>
        </div>
    </div>
</template>

<script setup lang="ts">
withDefaults(defineProps<{
    label?: string;
    disabled?: boolean;
    errors?: { $message: string; [key: string] }[];
}>(), {
    label: '',
    disabled: false,
    errors: () => [],
});
</script>
