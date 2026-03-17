<template>
    <div>
        <BasePageHeader
            icon="pi pi-exclamation-triangle"
            :title="isEdit ? 'Edit Incident Report' : 'New Incident Report'"
            :subtitle="isEdit ? store.form.incidentNumber ?? '' : 'Complete the incident report form'"
        >
            <BaseButtonCancel label="Cancel" @click="router.push(`/${apps.incidentManagement.baseSlug}/incidents`)" />
            <BaseButtonSave label="Save Draft" :loading="store.saving" @click="saveDraft" />
        </BasePageHeader>

        <div v-if="store.loading" class="flex justify-center py-12">
            <ProgressSpinner />
        </div>
        <div v-else>
            <IncidentFormPage1 />

            <div class="flex justify-end mt-6">
                <Button
                    label="Submit"
                    icon="pi pi-check"
                    :loading="store.saving"
                    @click="submitForm"
                />
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { apps } from '@/apps.ts';
import { useIncidentStore } from '@/modules/incident-management/stores/incidentStore';
import { useReferenceDataStore } from '@/modules/incident-management/stores/referenceDataStore';
import BasePageHeader from '@/components/layout/BasePageHeader.vue';
import BaseButtonCancel from '@/components/buttons/BaseButtonCancel.vue';
import BaseButtonSave from '@/components/buttons/BaseButtonSave.vue';
import IncidentFormPage1 from '@/modules/incident-management/features/incident-form/components/IncidentFormPage1.vue';

const router = useRouter();
const route = useRoute();
const store = useIncidentStore();
const refStore = useReferenceDataStore();

const isEdit = computed(() => !!route.params.id);

onMounted(async () => {
    store.resetForm();
    await refStore.loadAll();
    if (isEdit.value) {
        await store.loadIncident(route.params.id as string);
        if (store.form.companyId) {
            await refStore.loadRegions(store.form.companyId);
        }
    }
});

async function saveDraft(): Promise<boolean> {
    if (isEdit.value) {
        const updated = await store.updateIncident(route.params.id as string);
        return !!updated;
    } else {
        const created = await store.createIncident();
        if (created?.id) {
            router.replace(`/${apps.incidentManagement.baseSlug}/incidents/${created.id}`);
            return true;
        }
        return false;
    }
}

async function submitForm() {
    const previousStatus = store.form.status;
    store.form.status = 'SUBMIT';
    const saved = await saveDraft();
    if (saved) {
        router.push(`/${apps.incidentManagement.baseSlug}/incidents`);
    } else {
        store.form.status = previousStatus;
    }
}
</script>
