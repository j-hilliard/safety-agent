import {
    LogLevel,
    ServerError,
    AccountInfo,
    Configuration,
    PublicClientApplication,
    InteractionRequiredAuthError,
} from '@azure/msal-browser';

const tenantId = import.meta.env.VITE_TENANT_ID;
const appBaseUrl = import.meta.env.VITE_APP_BASE_URL;
const webClientId = import.meta.env.VITE_WEB_CLIENT_ID;
const apiClientId = import.meta.env.VITE_API_CLIENT_ID;

const scopes = [`api://${apiClientId}/default`, 'openid', 'profile', 'User.Read'];
const msalConfig: Configuration = {
    auth: {
        clientId: webClientId,
        postLogoutRedirectUri: appBaseUrl,
        authority: `https://login.microsoftonline.com/${tenantId}`,
        redirectUri: `${appBaseUrl}/authentication/login-callback`,
    },
    cache: { cacheLocation: 'localStorage', storeAuthStateInCookie: true, secureCookies: true },
    system: { loggerOptions: { logLevel: LogLevel.Info, piiLoggingEnabled: false } },
};

export const msalInstance = new PublicClientApplication(msalConfig);

export async function logout() {
    await msalInstance.logoutRedirect();
}

export async function login() {
    await msalInstance.loginRedirect({ scopes });
}

export async function handleRedirectResponse() {
    const currentAccounts = msalInstance.getAllAccounts();
    const response = await msalInstance.handleRedirectPromise();

    if (response && response.account) {
        return response.account;
    }

    if (currentAccounts.length === 1) {
        return currentAccounts[0];
    }

    return null;
}

export async function getToken(account: AccountInfo | null) {
    if (!account) {
        await msalInstance.loginRedirect({ scopes });
        return null;
    }

    try {
        const response = await msalInstance.acquireTokenSilent({ account, scopes });
        return response.accessToken;
    } catch (error) {
        const expiredTokenError = 'AADSTS700084';

        if (error instanceof InteractionRequiredAuthError) {
            await msalInstance.acquireTokenRedirect({ account, scopes });
            return null;
        } else if (error instanceof ServerError && error.message.includes(expiredTokenError)) {
            await msalInstance.loginRedirect({ scopes });
            return null;
        } else {
            throw error;
        }
    }
}
