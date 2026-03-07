import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import mkcert from 'vite-plugin-mkcert';
import { fileURLToPath, URL } from 'node:url';
import Components from 'unplugin-vue-components/vite';
import { PrimeVueResolver } from 'unplugin-vue-components/resolvers';

export default defineConfig(() => {
    return {
        build: { sourcemap: process.env.VITE_ENABLE_SOURCEMAP === 'true' },
        server: { port: 7208, https: true },
        plugins: [vue(), mkcert(), Components({ resolvers: [PrimeVueResolver()] })],
        resolve: { alias: { '@': fileURLToPath(new URL('./src', import.meta.url)) } },
    };
});
