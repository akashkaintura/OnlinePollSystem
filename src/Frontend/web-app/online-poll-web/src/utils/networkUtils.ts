import axios from 'axios';

export const NetworkService = {
    isOnline: (): boolean => navigator.onLine,

    checkInternetConnection: async (): Promise<boolean> => {
        try {
            await axios.get('https://www.google.com', { timeout: 5000 });
            return true;
        } catch {
            return false;
        }
    },

    createCancelToken: () => {
        const source = axios.CancelToken.source();
        return {
            token: source.token,
            cancel: source.cancel
        };
    }
};