<template>
    <BasePageHeader
        :title="apps.billingPacketRequestSystem.name"
        :subtitle="apps.billingPacketRequestSystem.description"
        :icon="apps.billingPacketRequestSystem.icon"
    />
</template>

<script setup lang="ts">
import { apps } from '@/apps';
import BasePageHeader from '@/components/layout/BasePageHeader.vue';
</script>