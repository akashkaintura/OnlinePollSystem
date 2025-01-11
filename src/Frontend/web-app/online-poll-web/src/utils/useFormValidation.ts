import { useState, useCallback } from 'react';

interface ValidationRule {
    validate: (value: any) => boolean;
    message: string;
}

export const useFormValidation = <T extends Record<string, any>>(
    initialValues: T,
    validationRules: Record<keyof T, ValidationRule[]>
) => {
    const [values, setValues] = useState<T>(initialValues);
    const [errors, setErrors] = useState<Partial<Record<keyof T, string[]>>>({});

    const handleChange = useCallback((
        field: keyof T,
        value: any
    ) => {
        setValues(prev => ({
            ...prev,
            [field]: value
        }));

        // Clear previous errors for this field
        setErrors(prev => ({
            ...prev,
            [field]: []
        }));
    }, []);

    const validate = useCallback(() => {
        const newErrors: Partial<Record<keyof T, string[]>> = {};
        let isValid = true;

        Object.keys(validationRules).forEach((field) => {
            const fieldRules = validationRules[field as keyof T];
            const value = values[field as keyof T];

            const fieldErrors = fieldRules
                .filter(rule => !rule.validate(value))
                .map(rule => rule.message);

            if (fieldErrors.length > 0) {
                newErrors[field as keyof T] = fieldErrors;
                isValid = false;
            }
        });

        setErrors(newErrors);
        return isValid;
    }, [values, validationRules]);

    const resetForm = useCallback(() => {
        setValues(initialValues);
        setErrors({});
    }, [initialValues]);

    return {
        values,
        errors,
        handleChange,
        validate,
        resetForm
    };
};

// Example Usage
const MyFormComponent = () => {
    const {
        values,
        errors,
        handleChange,
        validate,
        resetForm
    } = useFormValidation(
        {
            username: '',
            email: '',
            password: ''
        },
        {
            username: [
                {
                    validate: (value) => value.length >= 3,
                    message: 'Username must be at least 3 characters'
                }
            ],
            email: [
                {
                    validate: (value) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value),
                    message: 'Invalid email format'
                }
            ],
            password: [
                {
                    validate: (value) => value.length >= 8,
                    message: 'Password must be at least 8 characters'
                },
                {
                    validate: (value) => /[A-Z]/.test(value),
                    message: 'Password must contain an uppercase letter'
                }
            ]
        }
    );
};