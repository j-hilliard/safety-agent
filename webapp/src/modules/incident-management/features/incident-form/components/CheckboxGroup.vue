<template>
    <div class="flex flex-wrap gap-3">
        <div v-for="opt in options" :key="opt.id!" class="flex items-center gap-2">
            <Checkbox
                :inputId="`ref-${opt.id}`"
                :value="opt.id"
                :modelValue="store.form.referenceIds ?? []"
                @update:modelValue="store.form.referenceIds = $event"
            />
            <label :for="`ref-${opt.id}`" class="text-sm">{{ opt.name }}</label>
        </div>
        <span v-if="options.length === 0" class="text-sm text-slate-400">No options available.</span>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useIncidentStore } from '@/modules/incident-management/stores/incidentStore';
import { useReferenceDataStore } from '@/modules/incident-management/stores/referenceDataStore';

const props = defineProps<{ typeCode: string }>();
const store = useIncidentStore();
const refStore = useReferenceDataStore();

// Hardcoded fallback options per typeCode matching the SHC034 paper form.
// Used when DB reference tables are not yet seeded.
// Once seeded, DB values take over automatically.
const FALLBACK_OPTIONS: Record<string, { id: string; name: string }[]> = {
    environmental: [
        { id: 'environmental-emission', name: 'Emission' },
        { id: 'environmental-release', name: 'Release' },
    ],
    equipment_damage: [
        { id: 'equip-backing-up', name: 'Backing Up' },
        { id: 'equip-fire', name: 'Fire' },
        { id: 'equip-struck-by', name: 'Struck By' },
        { id: 'equip-explosion', name: 'Explosion' },
        { id: 'equip-malfunction', name: 'Malfunction' },
        { id: 'equip-unreported-damage', name: 'Unreported Damage' },
        { id: 'equip-windshield', name: 'Windshield' },
    ],
    injury_type: [
        { id: 'inj-caught-between', name: 'Caught Between/Under' },
        { id: 'inj-caught-in', name: 'Caught In' },
        { id: 'inj-caught-on', name: 'Caught On' },
        { id: 'inj-contact-with', name: 'Contact With' },
        { id: 'inj-exposure', name: 'Exposure' },
        { id: 'inj-fall-same-level', name: 'Fall Same Level' },
        { id: 'inj-fall-lower-level', name: 'Fall Lower Level' },
        { id: 'inj-overstress-ergonomic', name: 'Overstress, Ergonomic' },
        { id: 'inj-struck-against', name: 'Struck Against' },
        { id: 'inj-struck-by', name: 'Struck By' },
    ],
    injury_non_recordable: [
        { id: 'inj-nr-alleged', name: 'Alleged' },
        { id: 'inj-nr-first-aid', name: 'First Aid' },
        { id: 'inj-nr-report-only', name: 'Report Only' },
    ],
    injury_recordable: [
        { id: 'inj-r-medical-aid', name: 'Medical Aid' },
        { id: 'inj-r-modified-work', name: 'Modified Work' },
        { id: 'inj-r-lost-time', name: 'Lost Time' },
    ],
    life_support: [
        { id: 'ls-egress-cylinder', name: 'Egress Cylinder' },
        { id: 'ls-egress-regulator', name: 'Egress Regulator' },
        { id: 'ls-gas-detection', name: 'Gas Detection' },
        { id: 'ls-harness', name: 'Harness' },
        { id: 'ls-helmet-damage', name: 'Helmet Damage' },
        { id: 'ls-helmet-malfunction', name: 'Helmet Malfunction' },
        { id: 'ls-module', name: 'Module' },
        { id: 'ls-pigtail', name: 'Pigtail' },
        { id: 'ls-trailer-cube', name: 'Trailer/Cube' },
        { id: 'ls-umbilical', name: 'Umbilical' },
    ],
    motor_vehicle_accident: [
        { id: 'mva-animal-strike', name: 'Animal Strike' },
        { id: 'mva-single-vehicle', name: 'Single Vehicle' },
        { id: 'mva-third-party', name: 'Third Party' },
    ],
    policy_deviation: [
        { id: 'pd-client-policy', name: 'Client Policy' },
        { id: 'pd-life-saving-rule', name: 'Life-Saving Rule Violation' },
        { id: 'pd-shc-policy', name: 'SHC Policy' },
    ],
    security: [
        { id: 'sec-arson', name: 'Arson' },
        { id: 'sec-break-in', name: 'Break In' },
        { id: 'sec-theft', name: 'Theft' },
        { id: 'sec-vandalism', name: 'Vandalism' },
    ],
    transportation_compliance: [
        { id: 'tc-citations', name: 'Citations / Tickets' },
        { id: 'tc-public-driving-complaint', name: 'Public Driving Complaint' },
        { id: 'tc-roadside-inspection', name: 'Roadside Inspection' },
    ],
    investigation_required: [
        { id: 'inv-formal', name: 'Formal Investigation' },
        { id: 'inv-full-cause-map', name: 'Full Cause Map' },
    ],
};

const options = computed(() => {
    const dbOptions = refStore.getOptionsByType(props.typeCode);
    if (dbOptions.length > 0) return dbOptions;
    return FALLBACK_OPTIONS[props.typeCode] ?? [];
});
</script>
