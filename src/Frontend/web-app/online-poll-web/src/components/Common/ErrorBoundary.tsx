import React, { Component, ErrorInfo, ReactNode } from 'react';
import {
    Container,
    Typography,
    Button,
    Paper,
    Box
} from '@mui/material';
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';
import RefreshIcon from '@mui/icons-material/Refresh';

interface Props {
    children: ReactNode;
    fallback?: ReactNode;
    onError?: (error: Error, errorInfo: ErrorInfo) => void;
}

interface State {
    hasError: boolean;
    error?: Error;
    errorInfo?: ErrorInfo;
}

export class ErrorBoundary extends Component<Props, State> {
    public state: State = {
        hasError: false
    };

    public static getDerivedStateFromError(error: Error): State {
        return { hasError: true, error };
    }

    public componentDidCatch(error: Error, errorInfo: ErrorInfo) {
        // Log the error to an error reporting service
        console.error('Uncaught error:', error, errorInfo);

        // Call optional error handler if provided
        if (this.props.onError) {
            this.props.onError(error, errorInfo);
        }
    }

    private handleReload = () => {
        // Attempt to reload the page or reset the application state
        window.location.reload();
    };

    public render() {
        if (this.state.hasError) {
            // If a custom fallback is provided, use it
            if (this.props.fallback) {
                return this.props.fallback;
            }

            // Default error fallback UI
            return (
                <Container maxWidth="sm" sx={{ height: '100vh', display: 'flex', alignItems: 'center' }}>
                    <Paper
                        elevation={3}
                        sx={{
                            p: 4,
                            textAlign: 'center',
                            width: '100%',
                            backgroundColor: (theme) => theme.palette.background.default
                        }}
                    >
                        <Box sx={{ display: 'flex', justifyContent: 'center', mb: 3 }}>
                            <ErrorOutlineIcon
                                color="error"
                                sx={{ fontSize: 80 }}
                            />
                        </Box>

                        <Typography variant="h4" color="error" gutterBottom>
                            Oops! Something Went Wrong
                        </Typography>

                        <Typography variant="subtitle1" color="textSecondary" gutterBottom>
                            We're sorry, but an unexpected error occurred.
                        </Typography>

                        {this.state.error && (
                            <Box
                                sx={{
                                    backgroundColor: (theme) => theme.palette.error.light,
                                    color: (theme) => theme.palette.error.contrastText,
                                    p: 2,
                                    borderRadius: 1,
                                    mb: 3,
                                    overflowX: 'auto'
                                }}
                            >
                                <Typography variant="body2">
                                    Error Details: {this.state.error.message}
                                </Typography>
                            </Box>
                        )}

                        <Box sx={{ display: 'flex', justifyContent: 'center', gap: 2 }}>
                            <Button
                                variant="contained"
                                color="primary"
                                startIcon={<RefreshIcon />}
                                onClick={this.handleReload}
                            >
                                Reload Page
                            </Button>

                            <Button
                                variant="outlined"
                                color="secondary"
                                onClick={() => window.history.back()}
                            >
                                Go Back
                            </Button>
                        </Box>

                        <Typography
                            variant="caption"
                            color="textSecondary"
                            sx={{ mt: 2, display: 'block' }}
                        >
                            If the problem persists, please contact support.
                        </Typography>
                    </Paper>
                </Container>
            );
        }
        return this.props.children;
    }
}