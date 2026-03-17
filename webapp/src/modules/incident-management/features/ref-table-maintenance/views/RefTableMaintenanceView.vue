<template>
    <div>
        <BasePageHeader
            icon="pi pi-table"
            title="Reference Table Maintenance"
            subtitle="Manage companies, regions, and lookup tables"
        />

        <div class="mt-6">
            <TabView>
                <TabPanel header="Companies">
                    <CompaniesTab />
                </TabPanel>
                <TabPanel header="Regions">
                    <RegionsTab />
                </TabPanel>
                <TabPanel header="Lookup Tables">
                    <LookupTablesTab />
                </TabPanel>
            </TabView>
        </div>
    </div>
</template>

<script setup lang="ts">
import BasePageHeader from '@/components/layout/BasePageHeader.vue';
import CompaniesTab from '../components/CompaniesTab.vue';
import RegionsTab from '../components/RegionsTab.vue';
import LookupTablesTab from '../components/LookupTablesTab.vue';
</script>
