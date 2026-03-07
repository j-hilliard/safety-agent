<template>
  <div>
    <BasePageHeader title="401 Unauthorized" subtitle="You are not authorized to access this page."
                    icon="pi pi-lock" />
    <BasePageSubheader title="Please log in with proper credentials or contact your administrator for support." />
  </div>
</template>

<script setup lang="ts">
import BasePageHeader from '@/components/layout/BasePageHeader.vue';
import BasePageSubheader from '@/components/layout/BasePageSubheader.vue';
</script>