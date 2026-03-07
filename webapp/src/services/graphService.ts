import axios from 'axios';

export async function getUserProfilePhoto(token: string) {
    const response = await axios.get('https://graph.microsoft.com/v1.0/me/photo/$value', {
        responseType: 'blob',
        headers: { Authorization: `Bearer ${token}` },
    }).catch(() => null);

    return response?.data || null;
}
