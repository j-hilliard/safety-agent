<template>
    <div class="flex flex-col gap-6">

        <!-- Card 1: Initial Report Header Fields -->
        <Card>
            <template #title>Initial Report</template>
            <template #content>
                <!-- Row 1: Date | Company | Region -->
                <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4">
                    <BaseFormField label="Date *">
                        <Calendar v-model="store.form.incidentDate" showIcon class="w-full" dateFormat="mm/dd/yy" />
                    </BaseFormField>
                    <BaseFormField label="Stronghold Company *">
                        <Dropdown
                            v-model="store.form.companyId"
                            :options="refStore.companies"
                            optionLabel="name"
                            optionValue="id"
                            placeholder="Please Select"
                            class="w-full"
                            @change="onCompanyChange"
                        />
                        <div v-if="incidentNumberPreview" class="mt-1 text-xs text-slate-400">
                            Incident # format: <span class="text-blue-400 font-mono">{{ incidentNumberPreview }}</span>
                        </div>
                    </BaseFormField>
                    <BaseFormField label="SHC Region">
                        <Dropdown
                            v-model="store.form.regionId"
                            :options="refStore.regions"
                            optionLabel="name"
                            optionValue="id"
                            placeholder="Please Select"
                            class="w-full"
                            :disabled="!store.form.companyId"
                        />
                    </BaseFormField>
                </div>

                <!-- Unified grid: 3 content cols + 1 severity col, 2 rows -->
                <!-- Severity spans both rows via row-span-2 -->
                <div class="grid gap-4" style="grid-template-columns: 1fr 1fr 1fr 240px; grid-template-rows: auto 1fr;">

                    <!-- Row 1, Col 1-3: Job / Client / Plant -->
                    <BaseFormField label="Job Number">
                        <InputText v-model="store.form.jobNumber" class="w-full" />
                    </BaseFormField>
                    <BaseFormField label="Client Name">
                        <InputText v-model="store.form.clientCode" class="w-full" />
                    </BaseFormField>
                    <BaseFormField label="Plant Name">
                        <InputText v-model="store.form.plantCode" class="w-full" />
                    </BaseFormField>

                    <!-- Col 4, spans Row 1 + Row 2: Severity -->
                    <div class="row-span-2 flex flex-col">
                        <!-- Matches BaseFormField label style exactly -->
                        <label class="block text-sm font-medium text-slate-300 mb-1">Severity</label>
                        <div class="border border-slate-600 rounded overflow-hidden flex flex-col flex-1">
                            <!-- ACTUAL | POTENTIAL header — aligns with Job/Client/Plant row -->
                            <div class="flex bg-slate-700 shrink-0">
                                <div class="flex-1 py-2 text-xs font-bold text-white uppercase text-center border-r border-slate-500">Actual</div>
                                <div class="flex-1 py-2 text-xs font-bold text-white uppercase text-center">Potential</div>
                            </div>
                            <!-- Labels + radio buttons — fills Brief Description row height -->
                            <div class="flex flex-1">
                                <div class="flex-1 flex border-r border-slate-500">
                                    <div v-for="sev in severityOptions" :key="`sa-${sev.code}`"
                                         class="flex-1 flex flex-col items-center py-3 border-r border-slate-600 last:border-r-0 severity-radio-row">
                                        <span style="writing-mode: vertical-rl; transform: rotate(180deg); font-size: 14px;"
                                              class="text-slate-200 flex-1 flex items-center">{{ sev.name }}</span>
                                        <div class="mt-3 mb-1">
                                            <RadioButton v-model="store.form.severityActualCode"
                                                         :inputId="`sa-${sev.code}`" :value="sev.code" />
                                        </div>
                                    </div>
                                </div>
                                <div class="flex-1 flex">
                                    <div v-for="sev in severityOptions" :key="`sp-${sev.code}`"
                                         class="flex-1 flex flex-col items-center py-3 border-r border-slate-600 last:border-r-0 severity-radio-row">
                                        <span style="writing-mode: vertical-rl; transform: rotate(180deg); font-size: 14px;"
                                              class="text-slate-200 flex-1 flex items-center">{{ sev.name }}</span>
                                        <div class="mt-3 mb-1">
                                            <RadioButton v-model="store.form.severityPotentialCode"
                                                         :inputId="`sp-${sev.code}`" :value="sev.code" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Row 2, Col 1-3: Brief Description -->
                    <div class="col-span-3">
                        <BaseFormField label="Brief Description of Work">
                            <Textarea
                                v-model="store.form.workDescription"
                                rows="4"
                                class="w-full"
                                placeholder="Describe the work being performed at the time of the incident"
                                autoResize
                            />
                        </BaseFormField>
                    </div>

                </div>
            </template>
        </Card>

        <!-- Card 2: Initial Incident Details -->
        <Card>
            <template #title>Initial Incident Details</template>
            <template #content>
                <p class="text-sm text-slate-400 mb-6">
                    General incident information is used for the initial notification. This information can be updated
                    post-notification as more information becomes available.
                </p>

                <!-- Classification + Investigation Level -->
                <div class="grid grid-cols-1 md:grid-cols-2 gap-6 pb-6 mb-6 border-b border-slate-700">
                    <div>
                        <p class="text-xs font-bold text-white uppercase tracking-wide mb-3">Incident Classification</p>
                        <div class="flex flex-wrap gap-4">
                            <div v-for="opt in classificationOptions" :key="opt.value" class="flex items-center gap-2">
                                <RadioButton
                                    v-model="store.form.incidentClass"
                                    :inputId="`ic-${opt.value}`"
                                    :value="opt.value"
                                />
                                <label :for="`ic-${opt.value}`" class="text-sm">{{ opt.label }}</label>
                            </div>
                        </div>
                    </div>
                    <div>
                        <p class="text-xs font-bold text-white uppercase tracking-wide mb-3">Level of Formal Investigation Required</p>
                        <CheckboxGroup typeCode="investigation_required" />
                    </div>
                </div>

                <!-- Incident Type Table -->
                <div class="border border-slate-600 rounded-lg overflow-hidden text-sm">

                    <!-- Header row -->
                    <div class="grid grid-cols-[180px_1fr] bg-slate-700">
                        <div class="px-3 py-2 text-xs font-bold text-white uppercase tracking-wide border-r border-slate-600">
                            Incident Type
                        </div>
                        <div class="px-3 py-2 text-xs font-bold text-white uppercase tracking-wide">
                            Incident Character
                        </div>
                    </div>

                    <!-- Environmental -->
                    <div class="grid grid-cols-[180px_1fr] border-t border-slate-600">
                        <div class="px-3 py-3 font-semibold text-white border-r border-slate-600">Environmental</div>
                        <div class="px-3 py-3">
                            <CheckboxGroup typeCode="environmental" />
                        </div>
                    </div>

                    <!-- Equipment Damage -->
                    <div class="grid grid-cols-[180px_1fr] border-t border-slate-600">
                        <div class="px-3 py-3 font-semibold text-white border-r border-slate-600">Equipment Damage</div>
                        <div class="px-3 py-3">
                            <CheckboxGroup typeCode="equipment_damage" />
                            <div class="grid grid-cols-2 gap-2 mt-3">
                                <div>
                                    <label class="text-xs text-slate-400 block mb-1">Type of equipment</label>
                                    <InputText class="w-full" size="small" placeholder="Equipment type" />
                                </div>
                                <div>
                                    <label class="text-xs text-slate-400 block mb-1">Unit Numbers</label>
                                    <InputText class="w-full" size="small" placeholder="Unit #" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Injury -->
                    <div class="grid grid-cols-[180px_1fr] border-t border-slate-600">
                        <div class="px-3 py-3 font-semibold text-white border-r border-slate-600">Injury</div>
                        <div class="px-3 py-3">
                            <CheckboxGroup typeCode="injury_type" />
                            <div class="mt-3 grid grid-cols-2 gap-4">
                                <div>
                                    <p class="text-xs font-semibold text-slate-400 mb-2">Non-Recordable</p>
                                    <CheckboxGroup typeCode="injury_non_recordable" />
                                </div>
                                <div>
                                    <p class="text-xs font-semibold text-slate-400 mb-2">Recordable</p>
                                    <CheckboxGroup typeCode="injury_recordable" />
                                </div>
                            </div>
                            <div class="grid grid-cols-2 gap-2 mt-3">
                                <div>
                                    <label class="text-xs text-slate-400 block mb-1">Nature of Injury</label>
                                    <InputText class="w-full" size="small" placeholder="Nature of injury" />
                                </div>
                                <div>
                                    <label class="text-xs text-slate-400 block mb-1">Body Part Injured</label>
                                    <InputText class="w-full" size="small" placeholder="Body part" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Life Support -->
                    <div class="grid grid-cols-[180px_1fr] border-t border-slate-600">
                        <div class="px-3 py-3 font-semibold text-white border-r border-slate-600">Life Support</div>
                        <div class="px-3 py-3">
                            <CheckboxGroup typeCode="life_support" />
                        </div>
                    </div>

                    <!-- Motor Vehicle Accident -->
                    <div class="grid grid-cols-[180px_1fr] border-t border-slate-600">
                        <div class="px-3 py-3 font-semibold text-white border-r border-slate-600">Motor Vehicle Accident</div>
                        <div class="px-3 py-3">
                            <CheckboxGroup typeCode="motor_vehicle_accident" />
                            <div class="grid grid-cols-2 gap-2 mt-3">
                                <div>
                                    <label class="text-xs text-slate-400 block mb-1">Visibility</label>
                                    <InputText class="w-full" size="small" placeholder="Visibility conditions" />
                                </div>
                                <div>
                                    <label class="text-xs text-slate-400 block mb-1">Road Surface</label>
                                    <InputText class="w-full" size="small" placeholder="Road surface condition" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Policy Deviation -->
                    <div class="grid grid-cols-[180px_1fr] border-t border-slate-600">
                        <div class="px-3 py-3 font-semibold text-white border-r border-slate-600">Policy Deviation</div>
                        <div class="px-3 py-3">
                            <CheckboxGroup typeCode="policy_deviation" />
                        </div>
                    </div>

                    <!-- Security -->
                    <div class="grid grid-cols-[180px_1fr] border-t border-slate-600">
                        <div class="px-3 py-3 font-semibold text-white border-r border-slate-600">Security</div>
                        <div class="px-3 py-3">
                            <CheckboxGroup typeCode="security" />
                        </div>
                    </div>

                    <!-- Transportation Compliance -->
                    <div class="grid grid-cols-[180px_1fr] border-t border-slate-600">
                        <div class="px-3 py-3 font-semibold text-white border-r border-slate-600">Transportation Compliance</div>
                        <div class="px-3 py-3">
                            <CheckboxGroup typeCode="transportation_compliance" />
                        </div>
                    </div>

                </div>
            </template>
        </Card>

        <!-- Card 3: Incident Description Summary -->
        <Card>
            <template #title>Incident Description Summary</template>
            <template #content>
                <p class="text-sm text-slate-400 mb-3">
                    This is utilized for initial notification purposes, requiring a summary of the incident that
                    includes essential details.
                </p>
                <Textarea
                    v-model="store.form.incidentSummary"
                    rows="6"
                    class="w-full"
                    placeholder="Provide a detailed description of the incident"
                    autoResize
                />
            </template>
        </Card>

        <!-- Card 4: Involved Employees -->
        <Card>
            <template #title>Involved Employees</template>
            <template #content>
                <InvolvedEmployeeTable />
            </template>
        </Card>

    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useIncidentStore } from '@/modules/incident-management/stores/incidentStore';
import { useReferenceDataStore } from '@/modules/incident-management/stores/referenceDataStore';
import BaseFormField from '@/components/forms/BaseFormField.vue';
import InvolvedEmployeeTable from '@/modules/incident-management/features/incident-form/components/InvolvedEmployeeTable.vue';
import CheckboxGroup from '@/modules/incident-management/features/incident-form/components/CheckboxGroup.vue';

const store = useIncidentStore();
const refStore = useReferenceDataStore();

const incidentNumberPreview = computed(() => {
    if (!store.form.companyId) return null;
    const company = refStore.companies.find(c => c.id === store.form.companyId);
    if (!company?.code) return null;
    return `${company.code}-${new Date().getFullYear()}-????`;
});

const classificationOptions = [
    { label: 'Actual', value: 'Actual' },
    { label: 'Near Miss', value: 'NearMiss' },
    { label: 'Incident Report', value: 'IncidentReport' },
];

// Use DB-sourced severities when available, otherwise show fallback until data is seeded
const SEVERITY_FALLBACK = [
    { code: 'MINOR',   name: 'Minor' },
    { code: 'SERIOUS', name: 'Serious' },
    { code: 'MAJOR',   name: 'Major' },
];
const severityOptions = computed(() =>
    refStore.severities.length > 0
        ? refStore.severities.map(s => ({ code: s.code!, name: s.name! }))
        : SEVERITY_FALLBACK,
);

async function onCompanyChange() {
    store.form.regionId = undefined;
    if (store.form.companyId) {
        await refStore.loadRegions(store.form.companyId);
    }
}
</script>

<style scoped>
/* Make severity radio buttons visible against the dark card background */
.severity-radio-row :deep(.p-radiobutton .p-radiobutton-box) {
    background: #0f172a;
    border-color: #64748b;
    border-width: 2px;
    width: 22px;
    height: 22px;
}
.severity-radio-row :deep(.p-radiobutton .p-radiobutton-box:hover) {
    border-color: #93c5fd;
}
.severity-radio-row :deep(.p-radiobutton .p-radiobutton-icon) {
    width: 10px;
    height: 10px;
}
</style>
