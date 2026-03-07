<template>
  <Card class="mt-6">
    <template #content>
      <div v-if="$slots.filters" class="flex flex-col md:flex-row md:items-center gap-4 mb-4">
        <slot name="filters" />
      </div>
      <DataTable
        v-bind="$attrs"
        :value="value"
        :emptyMessage="emptyMessage"
        class="bg-navy-800/50 border border-slate-700 border-round-3xl"
      >
        <slot />
      </DataTable>
    </template>
  </Card>
</template>

<script setup lang="ts">
defineProps<{
  value: any[],
  emptyMessage?: string
}>();
</script> 