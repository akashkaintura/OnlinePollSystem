// src/components/Auth/Login.tsx
import React, { useState } from 'react';
import {
    TextField,
    Button,
    Container,
    Typography,
    Box,
    Alert
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useAuth } from './AuthContent';

export const Login: React.FC = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setLoading(true);

        try {
            // Validate inputs
            if (!email || !password) {
                setError('Please enter both email and password');
                setLoading(false);
                return;
            }

            // Attempt login
            await login(email, password);

            // Redirect to dashboard on successful login
            navigate('/dashboard');
        } catch (err: any) {
            // Handle different types of errors
            if (err.response) {
                // Server responded with an error
                setError(err.response.data.message || 'Login failed. Please try again.');
            } else if (err.request) {
                // Request was made but no response received
                setError('No response from server. Please check your connection.');
            } else {
                // Something happened in setting up the request
                setError('An unexpected error occurred. Please try again.');
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <Container maxWidth="xs">
            <Box
                sx={{
                    marginTop: 8,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}
            >
                <Typography component="h1" variant="h5">
                    Sign in
                </Typography>

                {error && (
                    <Alert
                        severity="error"
                        sx={{ width: '100%', mt: 2 }}
                        onClose={() => setError(null)}
                    >
                        {error}
                    </Alert>
                )}

                <Box
                    component="form"
                    onSubmit={handleSubmit}
                    sx={{ mt: 1, width: '100%' }}
                >
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        label="Email Address"
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        error={!!error}
                        disabled={loading}
                    />
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        label="Password"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        error={!!error}
                        disabled={loading}
                    />

                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{ mt: 3, mb: 2 }}
                        disabled={loading}
                    >
                        {loading ? 'Signing In...' : 'Sign In'}
                    </Button>

                    <Button
                        fullWidth
                        variant="outlined"
                        onClick={() => navigate('/register')}
                        disabled={loading}
                    >
                        Create New Account
                    </Button>
                </Box>
            </Box>
        </Container>
    );
};