import * as signalR from '@microsoft/signalr';
import { ref, onBeforeUnmount, onBeforeMount } from 'vue';

interface UseSignalRHubOptions {
    hub: string;
    events: { name: string, handler: (...args: []) => void }[];
}

export function useSignalRHub(options: UseSignalRHubOptions) {
    const connection = ref<signalR.HubConnection | null>(null);

    onBeforeMount(initializeConnection);

    onBeforeUnmount(async () => {
        if (connection.value) {
            await connection.value.stop()
                .catch(error => console.error(error))
                .finally(() => connection.value = null);
        }
    });

    function initializeConnection() {
        connection.value = new signalR.HubConnectionBuilder()
            .withUrl(`${import.meta.env.VITE_APP_API_BASE_URL}/${options.hub}`, { withCredentials: false })
            .withAutomaticReconnect()
            .build();

        for (const event of options.events) {
            connection.value.on(event.name, event.handler);
        }

        connection.value.start().catch((error) => console.error(error));
    }
}