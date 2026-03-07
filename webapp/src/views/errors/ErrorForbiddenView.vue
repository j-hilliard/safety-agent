<template>
  <div>
    <BasePageHeader title="403 Forbidden" subtitle="You do not have permission to access this page."
                    icon="pi pi-ban" />
    <BasePageSubheader title="Please check your credentials or contact your administrator for support." />
  </div>
</template>

<script setup lang="ts">
import BasePageHeader from '@/components/layout/BasePageHeader.vue';
import BasePageSubheader from '@/components/layout/BasePageSubheader.vue';
</script>