import React, { useState, useEffect } from 'react';
import {
    Container,
    Typography,
    Box,
    Avatar,
    Grid,
    Paper,
    Button
} from '@mui/material';
import { authService } from '../../services/authService';
import { pollService } from '../../services/pollService';
import { User } from '../../models/User';
import { Poll } from '../../models/Poll';

export const UserProfile: React.FC = () => {
    const [user, setUser] = useState<User | null>(null);
    const [createdPolls, setCreatedPolls] = useState<Poll[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                setLoading(true);
                const userData = await authService.getCurrentUser();
                setUser(userData);

                const polls = await pollService.getUserCreatedPolls();
                setCreatedPolls(polls);
            } catch (error) {
                console.error('Failed to fetch user data', error);
            } finally {
                setLoading(false);
            }
        };

        fetchUserData();
    }, []);

    if (loading) {
        return <Typography>Loading...</Typography>;
    }

    if (!user) {
        return <Typography>Unable to load user profile</Typography>;
    }

    return (
        <Container maxWidth="md">
            <Box sx={{ my: 4 }}>
                <Grid container spacing={3}>
                    <Grid item xs={12} md={4}>
                        <Paper elevation={3} sx={{ p: 3, textAlign: 'center' }}>
                            <Avatar
                                sx={{
                                    width: 100,
                                    height: 100,
                                    margin: '0 auto 16px',
                                    bgcolor: 'primary.main'
                                }}
                            >
                                {user.username.charAt(0).toUpperCase()}
                            </Avatar>
                            <Typography variant="h5">{user.username}</Typography>
                            <Typography variant="subtitle1">{user.email}</Typography>
                        </Paper>
                    </Grid>
                    <Grid item xs={12} md={8}>
                        <Paper elevation={3} sx={{ p: 3 }}>
                            <Typography variant="h6" gutterBottom>
                                Created Polls
                            </Typography>
                            {createdPolls.length === 0 ? (
                                <Typography>No polls created yet</Typography>
                            ) : (
                                createdPolls.map(poll => (
                                    <Box
                                        key={poll.id}
                                        sx={{
                                            mb: 2,
                                            p: 2,
                                            border: '1px solid',
                                            borderColor: 'grey.300',
                                            borderRadius: 2
                                        }}
                                    >
                                        <Typography variant="subtitle1">{poll.title}</Typography>
                                        <Typography variant="body2">
                                            Created on: {new Date(poll.createdAt).toLocaleDateString()}
                                        </Typography>
                                        <Button
                                            variant="outlined"
                                            size="small"
                                            sx={{ mt: 1 }}
                                            href={`/poll/${poll.id}/results`}
                                        >
                                            View Results
                                        </Button>
                                    </Box>
                                ))
                            )}
                        </Paper>
                    </Grid>
                </Grid>
            </Box>
        </Container>
    );
};