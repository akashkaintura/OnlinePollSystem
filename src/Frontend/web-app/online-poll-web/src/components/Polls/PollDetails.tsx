import React, { useState, useEffect } from 'react';
import {
    Container,
    Typography,
    Box,
    Button,
    RadioGroup,
    FormControlLabel,
    Radio
} from '@mui/material';
import { pollService } from '../../services/pollService';
import { useParams } from 'react-router-dom';

export const PollDetails: React.FC = () => {
    const { pollId } = useParams<{ pollId: string }>();
    const [poll, setPoll] = useState<any>(null);
    const [selectedOption, setSelectedOption] = useState<string>('');

    useEffect(() => {
        const fetchPollDetails = async () => {
            try {
                const data = await pollService.getPollDetails(Number(pollId));
                setPoll(data);
            } catch (error) {
                console.error('Failed to fetch poll details', error);
            }
        };

        fetchPollDetails();
    }, [pollId]);

    const handleVote = async () => {
        try {
            await pollService.votePoll(Number(pollId), Number(selectedOption));
            // Refresh poll or show success message
        } catch (error) {
            console.error('Failed to submit vote', error);
        }
    };

    if (!poll) return <Typography>Loading...</Typography>;

    return (
        <Container maxWidth="md">
            <Box sx={{ my: 4 }}>
                <Typography variant="h4">{poll.title}</Typography>
                <Typography variant="subtitle1">{poll.description}</Typography>

                <Box sx={{ my: 3 }}>
                    <Typography variant="h6">Vote for an option:</Typography>
                    <RadioGroup
                        value={selectedOption}
                        onChange={(e) => setSelectedOption(e.target.value)}
                    >
                        {poll.options.map((option: any) => (
                            <FormControlLabel
                                key={option.id}
                                value={option.id.toString()}
                                control={<Radio />}
                                label={option.optionText}
                            />
                        ))}
                    </RadioGroup>
                </Box>

                <Button
                    variant="contained"
                    color="primary"
                    onClick={handleVote}
                    disabled={!selectedOption}
                >
                    Submit Vote
                </Button>
            </Box>
        </Container>
    );
};
