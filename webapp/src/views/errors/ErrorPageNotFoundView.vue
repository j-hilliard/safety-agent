<template>
  <div>
    <BasePageHeader title="404 Page Not Found" subtitle="Oops, the page you're looking for doesn't exist."
                    icon="pi pi-search" />
    <BasePageSubheader title="Please check the url or contact your administrator for support." />
  </div>
</template>

<script setup lang="ts">
import BasePageHeader from '@/components/layout/BasePageHeader.vue';
import BasePageSubheader from '@/components/layout/BasePageSubheader.vue';
</script>