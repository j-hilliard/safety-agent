import { ref } from 'vue';
import axios from 'axios';
import router from '@/router';
import { defineStore } from 'pinia';
import { useUserStore } from '@/stores/userStore';
import { getToken } from '@/services/msalService';
import { AccountInfo } from '@azure/msal-browser';
import { useAppStore } from '@/stores/appStore.ts';

export const useApiStore = defineStore('api', () => {
    const isRefreshingToken = ref(false);
    const api = ref(axios.create({ baseURL: import.meta.env.VITE_APP_API_BASE_URL }));
    const failedQueue = ref<{ resolve: (token: string) => void; reject: (error) => void; }[]>([]);

    const appStore = useAppStore();

    function setApiInterceptors() {
        setApiRequestInterceptor();
        setApiResponseInterceptor();
    }

    async function setToken(user: AccountInfo | null) {
        setApiAuthorizationHeader(await getToken(user) || '');
        setApiInterceptors();
    }

    function setApiAuthorizationHeader(token: string) {
        api.value.defaults.headers.common.Authorization = `Bearer ${token}`;
    }

    function processQueue(error, token: string | null) {
        failedQueue.value.forEach(promise => token ? promise.resolve(token) : promise.reject(error));
        failedQueue.value = [];
    }

    function setApiResponseInterceptor() {
        api.value.interceptors.response.use(
            response => response,
            async (error) => {
                if (!error.response) {
                    return Promise.reject(error);
                }

                const { currentApp } = appStore;
                const { status } = error.response;
                const baseSlug = currentApp?.baseSlug ? `/${currentApp.baseSlug}` : '';

                if (status === 401) {
                    await router.push(`${baseSlug}/unauthorized`);
                }

                if (status === 403) {
                    await router.push(`${baseSlug}/forbidden`);
                }

                if (status >= 500 && status < 600) {
                    await router.push(`${baseSlug}/internal-server-error`);
                }

                return Promise.reject(error);
            },
        );
    }

    function setApiRequestInterceptor() {
        api.value.interceptors.request.use(async config => {
            const authorization = String(config.headers.getAuthorization());

            if (authorization?.startsWith('Bearer ')) {
                try {
                    const token = authorization.split(' ')[1];
                    const decodedToken = JSON.parse(atob(token.split('.')[1]));
                    const expirationDate = new Date(decodedToken.exp * 1000);
                    const bufferTime = 5 * 60 * 1000; // 5 minute buffer
                    const isTokenAlmostExpired = new Date().getTime() >= expirationDate.getTime() - bufferTime;

                    if (isTokenAlmostExpired) {
                        if (!isRefreshingToken.value) {
                            isRefreshingToken.value = true;

                            try {
                                const newToken = await getToken(useUserStore().userAccountInfo);
                                setApiAuthorizationHeader(newToken || '');
                                processQueue(null, newToken);
                                isRefreshingToken.value = false;
                            } catch (error) {
                                processQueue(error, null);
                                isRefreshingToken.value = false;
                                throw error;
                            }
                        }

                        return new Promise((resolve, reject) => {
                            failedQueue.value.push({
                                resolve: (newToken: string) => {
                                    config.headers.Authorization = `Bearer ${newToken}`;
                                    resolve(config);
                                },
                                reject: (error) => {
                                    reject(error);
                                },
                            });
                        });
                    }
                } catch {
                    return config;
                }
            }

            return config;
        }, error => Promise.reject(error));
    }

    return { api, setToken };
});
