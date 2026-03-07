<template>
  <Dialog
    :visible="visible"
    @update:visible="emit('update:visible', $event)"
    :header="header"
    :modal="true"
    :closable="true"
    :style="{ width: '500px', maxWidth: '100%' }"
    :draggable="false"
    v-bind="$attrs"
  >
    <div class="space-y-4">
      <slot />
    </div>
    <template #footer>
      <div class="flex justify-end gap-2">
        <slot name="footer" />
      </div>
    </template>
  </Dialog>
</template>

<script setup lang="ts">
const emit = defineEmits(['update:visible']);

defineProps<{
  visible: boolean,
  header: string
}>();
</script> 