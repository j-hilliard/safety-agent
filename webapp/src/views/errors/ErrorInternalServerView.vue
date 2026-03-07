<template>
  <div>
    <BasePageHeader title="500 Internal Server Error" subtitle="Something went wrong on our end."
                    icon="pi pi-times-circle" />
    <BasePageSubheader title="Please try again later or contact support if the issue persists." />
  </div>
</template>

<script setup lang="ts">
import BasePageHeader from '@/components/layout/BasePageHeader.vue';
import BasePageSubheader from '@/components/layout/BasePageSubheader.vue';
</script>