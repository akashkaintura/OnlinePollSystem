class CacheService {
    private cache: Map<string, { data: any; timestamp: number }> = new Map();
    private MAX_CACHE_TIME = 5 * 60 * 1000;

    set(key: string, data: any, customTTL?: number) {
        const timestamp = Date.now();
        this.cache.set(key, {
            data,
            timestamp
        });
    }

    get(key: string) {
        const cached = this.cache.get(key);
        if (!cached) return null;

        const isExpired =
            Date.now() - cached.timestamp > this.MAX_CACHE_TIME;

        if (isExpired) {
            this.cache.delete(key);
            return null;
        }

        return cached.data;
    }

    delete(key: string) {
        this.cache.delete(key);
    }

    clear() {
        this.cache.clear();
    }
}

export const cacheService = new CacheService();