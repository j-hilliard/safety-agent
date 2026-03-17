import { computed } from 'vue';
import { useReferenceDataStore } from '@/modules/incident-management/stores/referenceDataStore';
import type { RefOptionDto } from '@/apiclient/client';

export function useReferenceOptions(typeCode: string) {
    const refStore = useReferenceDataStore();
    const options = computed<RefOptionDto[]>(() => refStore.getOptionsByType(typeCode));
    return { options };
}
