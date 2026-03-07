<template>
    <div class="pdf-viewer">
        <iframe v-if="pdfUrl" :src="formattedUrl" />
        <div v-else class="error">Invalid PDF URL</div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const pdfUrl = computed(() => {
    const raw = String(route.query.pdf) || '';
    return raw ? decodeURIComponent(raw) : '';
});

const formattedUrl = computed(() => {
    if (!pdfUrl.value) {
        return '';
    }

    if (pdfUrl.value.includes('?')) {
        return `${pdfUrl.value}&embedded=true`;
    }

    return `${pdfUrl.value}?embedded=true`;
});
</script>

<style scoped>
.error {
    color: red;
    font-size: 1.2rem;
}

.pdf-viewer iframe {
    width: 100%;
    height: 100%;
    border: none;
}

.pdf-viewer {
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    display: flex;
    position: fixed;
    background: #f4f4f4;
    align-items: center;
    justify-content: center;
}
</style>
