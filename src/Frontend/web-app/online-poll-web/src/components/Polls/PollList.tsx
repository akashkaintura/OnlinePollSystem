import React, { useState, useEffect } from 'react';
import {
    Container,
    Typography,
    Grid,
    Card,
    CardContent,
    CardActions,
    Button,
    Pagination,
    Box,
    Chip,
    LinearProgress
} from '@mui/material';
import { pollService } from '../../services/pollService';
import { Poll, PaginationResult } from '../../models/Poll';
import { useNavigate } from 'react-router-dom';

export const PollList: React.FC = () => {
    const [polls, setPolls] = useState<PaginationResult<Poll>>({
        items: [],
        totalItems: 0,
        pageNumber: 1,
        pageSize: 10,
        totalPages: 0
    });
    const [loading, setLoading] = useState(true);
    const [page, setPage] = useState(1);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchPolls = async () => {
            try {
                setLoading(true);
                const response = await pollService.getActivePolls(page);
                setPolls(response);
            } catch (error) {
                console.error('Failed to fetch polls', error);
            } finally {
                setLoading(false);
            }
        };

        fetchPolls();
    }, [page]);

    const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value);
    };

    const handlePollDetails = (pollId: number) => {
        navigate(`/poll/${pollId}`);
    };

    if (loading) {
        return <LinearProgress />;
    }

    return (
        <Container maxWidth="lg" sx={{ mt: 4 }}>
            <Typography variant="h4" gutterBottom>
                Active Polls
            </Typography>

            <Grid container spacing={3}>
                {polls.items.map((poll) => (
                    <Grid item xs={12} md={6} lg={4} key={poll.id}>
                        <Card variant="outlined">
                            <CardContent>
                                <Typography variant="h6" gutterBottom>
                                    {poll.title}
                                </Typography>
                                <Typography variant="body2" color="textSecondary">
                                    {poll.description}
                                </Typography>
                                <Box sx={{ mt: 2, display: 'flex', justifyContent: 'space-between' }}>
                                    <Chip
                                        label={`Options: ${poll.options.length}`}
                                        size="small"
                                        color="primary"
                                    />
                                    <Chip
                                        label={poll.isActive ? 'Active' : 'Closed'}
                                        size="small"
                                        color={poll.isActive ? 'success' : 'default'}
                                    />
                                </Box>
                            </CardContent>
                            <CardActions>
                                <Button
                                    size="small"
                                    color="primary"
                                    onClick={() => handlePollDetails(poll.id)}
                                >
                                    View Details
                                </Button>
                                {poll.isActive && (
                                    <Button
                                        size="small"
                                        color="secondary"
                                        onClick={() => navigate(`/poll/${poll.id}/vote`)}
                                    >
                                        Vote
                                    </Button>
                                )}
                            </CardActions>
                        </Card>
                    </Grid>
                ))}
            </Grid>

            {polls.totalPages > 1 && (
                <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
                    <Pagination
                        count={polls.totalPages}
                        page={page}
                        onChange={handlePageChange}
                        color="primary"
                    />
                </Box>
            )}

            {polls.items.length === 0 && (
                <Box sx={{ textAlign: 'center', mt: 4 }}>
                    <Typography variant="h6" color="textSecondary">
                        No active polls available
                    </Typography>
                </Box>
            )}
        </Container>
    );
};