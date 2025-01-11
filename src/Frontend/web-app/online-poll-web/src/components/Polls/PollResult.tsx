import React, { useState, useEffect } from 'react';
import {
    Container,
    Typography,
    Box,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Paper,
    LinearProgress
} from '@mui/material';
import { pollService } from '../../services/pollService';
import { PollResult } from '../../models/Poll';
import { useParams } from 'react-router-dom';

export const PollResults: React.FC = () => {
    const { pollId } = useParams<{ pollId: string }>();
    const [pollResults, setPollResults] = useState<PollResult | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchPollResults = async () => {
            try {
                setLoading(true);
                const results = await pollService.getPollResults(Number(pollId));
                setPollResults(results);
            } catch (error) {
                console.error('Failed to fetch poll results', error);
            } finally {
                setLoading(false);
            }
        };

        fetchPollResults();
    }, [pollId]);

    if (loading) {
        return <LinearProgress />;
    }

    if (!pollResults) {
        return <Typography>No results available</Typography>;
    }

    return (
        <Container maxWidth="md">
            <Box sx={{ my: 4 }}>
                <Typography variant="h4" gutterBottom>
                    Poll Results: {pollResults.title}
                </Typography>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Option</TableCell>
                                <TableCell align="right">Votes</TableCell>
                                <TableCell align="right">Percentage</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {pollResults.optionResults.map((option) => (
                                <TableRow key={option.optionId}>
                                    <TableCell component="th" scope="row">
                                        {option.optionText}
                                    </TableCell>
                                    <TableCell align="right">{option.voteCount}</TableCell>
                                    <TableCell align="right">
                                        {(option.percentage * 100).toFixed(2)}%
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>
        </Container>
    );
};