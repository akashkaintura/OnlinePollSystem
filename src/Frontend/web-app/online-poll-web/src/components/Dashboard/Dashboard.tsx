import React, { useState, useEffect } from 'react';
import {
    Container,
    Typography,
    Grid,
    Paper,
    Box,
    Button,
    Card,
    CardContent,
    CardActions,
    LinearProgress
} from '@mui/material';
import {
    PollOutlined as PollIcon,
    AddCircleOutline as CreatePollIcon,
    Person as ProfileIcon,
    InsertChart as ResultsIcon
} from '@mui/icons-material';
import { Link } from 'react-router-dom';
import { useAuth } from '../Auth/AuthContent';
import { pollService } from '../../services/pollService';
import { Poll } from '../../models/Poll';

export const Dashboard: React.FC = () => {
    const { user } = useAuth();
    const [recentPolls, setRecentPolls] = useState<Poll[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchRecentPolls = async () => {
            try {
                // Fetch recent polls (adjust the method based on your backend)
                const pollsResponse = await pollService.getActivePolls(1, 5);
                setRecentPolls(pollsResponse.items);
            } catch (error) {
                console.error('Failed to fetch recent polls', error);
            } finally {
                setLoading(false);
            }
        };

        fetchRecentPolls();
    }, []);

    return (
        <Container maxWidth="lg" sx={{ mt: 4 }}>
            <Typography variant="h4" gutterBottom>
                Welcome, {user?.username || 'User'}
            </Typography>

            <Grid container spacing={3}>
                {/* Quick Actions */}
                <Grid item xs={12} md={4}>
                    <Paper
                        elevation={3}
                        sx={{
                            p: 3,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            height: '100%'
                        }}
                    >
                        <Typography variant="h6" gutterBottom>
                            Quick Actions
                        </Typography>
                        <Grid container spacing={2}>
                            <Grid item xs={6}>
                                <Button
                                    fullWidth
                                    variant="outlined"
                                    startIcon={<CreatePollIcon />}
                                    component={Link}
                                    to="/create-poll"
                                >
                                    Create Poll
                                </Button>
                            </Grid>
                            <Grid item xs={6}>
                                <Button
                                    fullWidth
                                    variant="outlined"
                                    startIcon={<PollIcon />}
                                    component={Link}
                                    to="/polls"
                                >
                                    View Polls
                                </Button>
                            </Grid>
                            <Grid item xs={6}>
                                <Button
                                    fullWidth
                                    variant="outlined"
                                    startIcon={<ProfileIcon />}
                                    component={Link}
                                    to="/profile"
                                >
                                    Profile
                                </Button>
                            </Grid>
                            <Grid item xs={6}>
                                <Button
                                    fullWidth
                                    variant="outlined"
                                    startIcon={<ResultsIcon />}
                                    component={Link}
                                    to="/polls/results"
                                >
                                    Results
                                </Button>
                            </Grid>
                        </Grid>
                    </Paper>
                </Grid>

                {/* Recent Polls */}
                <Grid item xs={12} md={8}>
                    <Paper elevation={3} sx={{ p: 3 }}>
                        <Typography variant="h6" gutterBottom>
                            Recent Polls
                        </Typography>
                        {loading ? (
                            <LinearProgress />
                        ) : (
                            recentPolls.length === 0 ? (
                                <Typography variant="body2" color="textSecondary">
                                    No recent polls available
                                </Typography>
                            ) : (
                                <Grid container spacing={2}>
                                    {recentPolls.map((poll) => (
                                        <Grid item xs={12} key={poll.id}>
                                            <Card variant="outlined">
                                                <CardContent>
                                                    <Typography variant="subtitle1">
                                                        {poll.title}
                                                    </Typography>
                                                    <Typography variant="body2" color="textSecondary">
                                                        Created on: {new Date(poll.createdAt).toLocaleDateString()}
                                                    </Typography>
                                                </CardContent>
                                                <CardActions>
                                                    <Button
                                                        size="small"
                                                        color="primary"
                                                        component={Link}
                                                        to={`/poll/${poll.id}`}
                                                    >
                                                        View Poll
                                                    </Button>
                                                    <Button
                                                        size="small"
                                                        color="secondary"
                                                        component={Link}
                                                        to={`/poll/${poll.id}/results`}
                                                    >
                                                        View Results
                                                    </Button>
                                                </CardActions>
                                            </Card>
                                        </Grid>
                                    ))}
                                </Grid>
                            )
                        )}
                    </Paper>
                </Grid>

                {/* Statistics */}
                <Grid item xs={12}>
                    <Paper elevation={3} sx={{ p: 3 }}>
                        <Typography variant="h6" gutterBottom>
                            Your Poll Statistics
                        </Typography>
                        <Grid container spacing={2}>
                            <Grid item xs={12} md={4}>
                                <Card variant="outlined">
                                    <CardContent>
                                        <Typography variant="h5">
                                            {recentPolls.length}
                                        </Typography>
                                        <Typography variant="subtitle2" color="textSecondary">
                                            Total Polls
                                        </Typography>
                                    </CardContent>
                                </Card>
                            </Grid>
                            <Grid item xs={12} md={4}>
                                <Card variant="outlined">
                                    <CardContent>
                                        <Typography variant="h5">
                                            {/* Add logic to calculate active polls */}
                                            {recentPolls.filter(poll => poll.isActive).length}
                                        </Typography>
                                        <Typography variant="subtitle2" color="textSecondary">
                                            Active Polls
                                        </Typography>
                                    </CardContent>
                                </Card>
                            </Grid>
                            <Grid item xs={12} md={4}>
                                <Card variant="outlined">
                                    <CardContent>
                                        <Typography variant="h5">
                                            {/* Add logic to calculate completed polls */}
                                            {recentPolls.filter(poll => !poll.isActive).length}
                                        </Typography>
                                        <Typography variant="subtitle2" color="textSecondary">
                                            Completed Polls
                                        </Typography>
                                    </CardContent>
                                </Card>
                            </Grid>
                        </Grid>
                    </Paper>
                </Grid>
            </Grid>
        </Container>
    );
};