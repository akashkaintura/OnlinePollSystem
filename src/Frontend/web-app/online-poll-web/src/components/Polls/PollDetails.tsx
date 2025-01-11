import React, { useState, useEffect } from 'react';
import {
    Container,
    Typography,
    Box,
    Paper,
    Button,
    Radio,
    RadioGroup,
    FormControlLabel,
    FormControl,
    FormLabel,
    LinearProgress,
    Alert
} from '@mui/material';
import { useParams, useNavigate } from 'react-router-dom';
import { pollService } from '../../services/pollService';
import { Poll } from '../../models/Poll';

export const PollDetails: React.FC = () => {
    const { pollId } = useParams<{ pollId: string }>();
    const navigate = useNavigate();

    const [poll, setPoll] = useState<Poll | null>(null);
    const [selectedOption, setSelectedOption] = useState<number | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [voting, setVoting] = useState(false);

    useEffect(() => {
        const fetchPollDetails = async () => {
            try {
                setLoading(true);
                const pollDetails = await pollService.getPollDetails(Number(pollId));
                setPoll(pollDetails);
            } catch (err: any) {
                setError(err.response?.data?.message || 'Failed to fetch poll details');
            } finally {
                setLoading(false);
            }
        };

        fetchPollDetails();
    }, [pollId]);

    const handleVote = async () => {
        if (!selectedOption) {
            setError('Please select an option');
            return;
        }

        try {
            setVoting(true);
            await pollService.votePoll(Number(pollId), selectedOption);
            navigate(`/poll/${pollId}/results`);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Voting failed');
        } finally {
            setVoting(false);
        }
    };

    if (loading) {
        return <LinearProgress />;
    }

    if (!poll) {
        return <Typography>Poll not found</Typography>;
    }

    return (
        <Container maxWidth="md" sx={{ mt: 4 }}>
            <Paper elevation={3} sx={{ p: 4 }}>
                <Typography variant="h4" gutterBottom>
                    {poll.title}
                </Typography>

                <Typography variant="body1" sx={{ mb: 3 }}>
                    {poll.description}
                </Typography>

                {error && (
                    <Alert
                        severity="error"
                        sx={{ mb: 2 }}
                        onClose={() => setError(null)}
                    >
                        {error}
                    </Alert>
                )}

                <FormControl component="fieldset" fullWidth>
                    <FormLabel component="legend">Select an option</FormLabel>
                    <RadioGroup
                        value={selectedOption}
                        onChange={(e) => setSelectedOption(Number(e.target.value))}
                    >
                        {poll.options.map((option) => (
                            <FormControlLabel
                                key={option.id}
                                value={option.id}
                                control={<Radio />}
                                label={option.optionText}
                                disabled={!poll.isActive || voting}
                            />
                        ))}
                    </RadioGroup>
                </FormControl>

                <Box sx={{ mt: 3, display: 'flex', justifyContent: 'space-between' }}>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={handleVote}
                        disabled={!poll.isActive || !selectedOption || voting}
                    >
                        {voting ? 'Voting...' : 'Submit Vote'}
                    </Button>
                    <Button
                        variant="outlined"
                        color="secondary"
                        onClick={() => navigate('/polls')}
                    >
                        Back to Polls
                    </Button>
                </Box>

                {!poll.isActive && (
                    <Alert severity="warning" sx={{ mt: 2 }}>
                        This poll is no longer active
                    </Alert>
                )}
            </Paper>
        </Container>
    );
};