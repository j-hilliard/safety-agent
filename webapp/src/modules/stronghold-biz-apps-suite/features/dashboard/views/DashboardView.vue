<template>
    <div>
        <BasePageHeader
            :title="apps.strongholdBizAppsSuite.name"
            :subtitle="apps.strongholdBizAppsSuite.description"
            :icon="apps.strongholdBizAppsSuite.icon"
        />
        <BasePageSubheader title="Available Applications" />
        <div class="grid md:grid-cols-3 lg:grid-cols-4 gap-4 mt-6">
            <router-link v-for="app in filteredApps" :key="app.name" :to="app.baseSlug" class="no-underline">
                <Card class="hover:shadow-lg transition-shadow">
                    <template #content>
                        <div class="flex items-center mb-4">
                            <i :class="[app.icon, 'text-4xl text-blue-400 mr-4']" />
                            <div>
                                <h3 class="text-xl font-medium">{{ app.name }}</h3>
                                <p class="text-gray-400 text-sm">{{ app.description }}</p>
                            </div>
                        </div>
                    </template>
                </Card>
            </router-link>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { apps } from '@/apps.ts';
import BasePageSubheader from '@/components/layout/BasePageSubheader.vue';
import BasePageHeader from '@/components/layout/BasePageHeader.vue';

const filteredApps = computed(() => {
    return Object.values(apps).filter(app => app.name !== apps.strongholdBizAppsSuite.name);
});
</script>
