import React from 'react';
import {
    Container,
    Grid,
    Paper,
    Typography,
    Button
} from '@mui/material';
import { Link } from 'react-router-dom';

export const Dashboard: React.FC = () => {
    return (
        <Container maxWidth="lg" sx={{ mt: 4 }}>
            <Typography variant="h4" gutterBottom>
                Dashboard
            </Typography>
            <Grid container spacing={3}>
                <Grid item xs={12} md={4}>
                    <Paper
                        sx={{
                            p: 3,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center'
                        }}
                    >
                        <Typography variant="h6">Create a New Poll</Typography>
                        <Button
                            component={Link}
                            to="/create-poll"
                            variant="contained"
                            sx={{ mt: 2 }}
                        >
                            Create Poll
                        </Button>
                    </Paper>
                </Grid>
                <Grid item xs={12} md={4}>
                    <Paper
                        sx={{
                            p: 3,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center'
                        }}
                    >
                        <Typography variant="h6">View Active Polls</Typography>
                        <Button
                            component={Link}
                            to="/polls"
                            variant="contained"
                            sx={{ mt: 2 }}
                        >
                            Browse Polls
                        </Button>
                    </Paper>
                </Grid>
                <Grid item xs={12} md={4}>
                    <Paper
                        sx={{
                            p: 3,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center'
                        }}
                    >
                        <Typography variant="h6">My Profile</Typography>
                        <Button
                            component={Link}
                            to="/profile"
                            variant="contained"
                            sx={{ mt: 2 }}
                        >
                            View Profile
                        </Button>
                    </Paper>
                </Grid>
            </Grid>
        </Container>
    );
};