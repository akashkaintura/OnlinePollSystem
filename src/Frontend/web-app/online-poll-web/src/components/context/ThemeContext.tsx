import React, { createContext, useState, useContext, useMemo } from 'react';
import {
    ThemeProvider as MUIThemeProvider,
    createTheme,
    Theme
} from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';

interface ThemeContextType {
    toggleColorMode: () => void;
    mode: 'light' | 'dark';
}

const ThemeContext = createContext<ThemeContextType>({
    toggleColorMode: () => { },
    mode: 'light'
});

export const ThemeProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [mode, setMode] = useState<'light' | 'dark'>('light');

    const colorMode = useMemo(
        () => ({
            toggleColorMode: () => {
                setMode((prevMode) => (prevMode === 'light' ? 'dark' : 'light'));
            },
            mode
        }),
        [mode]
    );

    const theme = useMemo(
        () =>
            createTheme({
                palette: {
                    mode,
                    ...(mode === 'light'
                        ? {
                            // Light theme customizations
                            primary: { main: '#1976d2' },
                            secondary: { main: '#dc004e' },
                        }
                        : {
                            // Dark theme customizations
                            primary: { main: '#90caf9' },
                            secondary: { main: '#f48fb1' },
                        }),
                },
                typography: {
                    fontFamily: 'Roboto, Arial, sans-serif',
                },
                components: {
                    MuiCssBaseline: {
                        styleOverrides: `
              body {
                transition: background-color 0.3s ease;
              }
            `,
                    },
                },
            }),
        [mode]
    );

    return (
        <ThemeContext.Provider value={colorMode}>
            <MUIThemeProvider theme={theme}>
                <CssBaseline />
                {children}
            </MUIThemeProvider>
        </ThemeContext.Provider>
    );
};

export const useThemeMode = () => useContext(ThemeContext);