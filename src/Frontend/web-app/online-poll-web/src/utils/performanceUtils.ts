export const PerformanceMonitor = {
    measureExecutionTime: <T>(fn: () => T): { result: T; executionTime: number } => {
        const start = performance.now();
        const result = fn();
        const end = performance.now();
        return {
            result,
            executionTime: end - start
        };
    },

    debounce: <F extends (...args: any[]) => any>(
        func: F,
        delay: number
    ): ((...args: Parameters<F>) => void) => {
        let timeoutId: ReturnType<typeof setTimeout> | null = null;

        return (...args: Parameters<F>) => {
            if (timeoutId) {
                clearTimeout(timeoutId);
            }

            timeoutId = setTimeout(() => {
                func(...args);
            }, delay);
        };
    },

    throttle: <F extends (...args: any[]) => any>(
        func: F,
        limit: number
    ): ((...args: Parameters<F>) => void) => {
        let inThrottle: boolean;
        return (...args: Parameters<F>) => {
            if (!inThrottle) {
                func(...args);
                inThrottle = true;
                setTimeout(() => inThrottle = false, limit);
            }
        };
    }
};
