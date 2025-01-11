export const FormatUtils = {
    formatCurrency: (value: number, locale: string = 'en-US', currency: string = 'USD') => {
        return new Intl.NumberFormat(locale, {
            style: 'currency',
            currency: currency
        }).format(value);
    },

    truncateText: (text: string, maxLength: number = 100) => {
        if (text.length <= maxLength) return text;
        return `${text.substring(0, maxLength)}...`;
    },

    capitalizeFirstLetter: (str: string) => {
        return str.charAt(0).toUpperCase() + str.slice(1);
    }
};