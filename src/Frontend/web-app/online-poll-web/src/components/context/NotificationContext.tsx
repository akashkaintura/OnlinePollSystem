import React, { createContext, useState, useContext, ReactNode } from 'react';
import { Snackbar, Alert } from '@mui/material';

interface NotificationContextType {
    showSuccess: (message: string) => void;
    showError: (message: string) => void;
    showWarning: (message: string) => void;
    showInfo: (message: string) => void;
}

const NotificationContext = createContext<NotificationContextType>({
    showSuccess: () => { },
    showError: () => { },
    showWarning: () => { },
    showInfo: () => { }
});

export const NotificationProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [open, setOpen] = useState(false);
    const [message, setMessage] = useState('');
    const [severity, setSeverity] = useState<'success' | 'error' | 'warning' | 'info'>('info');

    const handleClose = (event?: React.SyntheticEvent | Event, reason?: string) => {
        if (reason === 'clickaway') {
            return;
        }
        setOpen(false);
    };

    const showNotification = (msg: string, type: 'success' | 'error' | 'warning' | 'info') => {
        setMessage(msg);
        setSeverity(type);
        setOpen(true);
    };

    return (
        <NotificationContext.Provider
            value={{
                showSuccess: (msg) => showNotification(msg, 'success'),
                showError: (msg) => showNotification(msg, 'error'),
                showWarning: (msg) => showNotification(msg, 'warning'),
                showInfo: (msg) => showNotification(msg, 'info')
            }}
        >
            {children}
            <Snackbar
                open={open}
                autoHideDuration={6000}
                onClose={handleClose}
                anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
            >
                <Alert
                    onClose={handleClose}
                    severity={severity}
                    sx={{ width: '100%' }}
                >
                    {message}
                </Alert>
            </Snackbar>
        </NotificationContext.Provider>
    );
};

export const useNotification = () => useContext(NotificationContext);