<template>
    <div
        class="auth-redirect flex items-center justify-center h-screen flex-col bg-gradient-to-b from-slate-900 to-slate-800">
        <h1 class="text-4xl font-bold text-slate-100 mb-6">Redirecting...</h1>
        <p class="text-lg text-slate-300 mb-10 text-center">
            Please hold on while we authenticate your access. This won’t take long.
        </p>
        <div class="loader">
            <div class="loader-inner">
                <div v-for="n in 5" :key="n" class="loader-line-wrap">
                    <div class="loader-line"></div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { handleRedirectResponse } from '@/services/msalService.ts';

const router = useRouter();

onMounted(async () => {
    try {
        await handleRedirectResponse();
        await router.push('/');
    } catch (error) {
        console.error(error);
    }
});
</script>
