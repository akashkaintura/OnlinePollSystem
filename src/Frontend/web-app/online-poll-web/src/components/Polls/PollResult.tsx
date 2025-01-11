import React, { useState, useEffect } from 'react';
import {
    Container,
    Typography,
    Box,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    LinearProgress,
    Grid,
    Card,
    CardContent,
    Chip
} from '@mui/material';
import { PieChart, Pie, Cell, ResponsiveContainer, Tooltip, Legend } from 'recharts';
import { useParams } from 'react-router-dom';
import { pollService } from '../../services/pollService';
import { PollResult } from '../../models/Poll';

// Generate random colors for chart
const COLORS = [
    '#0088FE', '#00C49F', '#FFBB28', '#FF8042',
    '#8884D8', '#82CA9D', '#FF6384', '#36A2EB'
];

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

    // Prepare data for pie chart
    const chartData = pollResults.optionResults.map((option, index) => ({
        name: option.optionText,
        value: option.voteCount,
        color: COLORS[index % COLORS.length]
    }));

    return (
        <Container maxWidth="lg" sx={{ mt: 4 }}>
            <Paper elevation={3} sx={{ p: 4 }}>
                <Typography variant="h4" gutterBottom>
                    Poll Results: {pollResults.title}
                </Typography>

                <Grid container spacing={3}>
                    {/* Pie Chart */}
                    <Grid item xs={12} md={6}>
                        <Card variant="outlined">
                            <CardContent>
                                <Typography variant="h6" gutterBottom>
                                    Vote Distribution
                                </Typography>
                                <ResponsiveContainer width="100%" height={300}>
                                    <PieChart>
                                        <Pie
                                            data={chartData}
                                            cx="50%"
                                            cy="50%"
                                            labelLine={false}
                                            outerRadius={120}
                                            fill="#8884d8"
                                            dataKey="value"
                                        >
                                            {chartData.map((entry, index) => (
                                                <Cell
                                                    key={`cell-${index}`}
                                                    fill={entry.color}
                                                />
                                            ))}
                                        </Pie>
                                        <Tooltip />
                                        <Legend />
                                    </PieChart>
                                </ResponsiveContainer>
                            </CardContent>
                        </Card>
                    </Grid>

                    {/* Results Table */}
                    <Grid item xs={12} md={6}>
                        <Card variant="outlined">
                            <CardContent>
                                <Typography variant="h6" gutterBottom>
                                    Detailed Results
                                </Typography>
                                <TableContainer>
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
                                                    <TableCell align="right">
                                                        <Chip
                                                            label={option.voteCount}
                                                            size="small"
                                                            color="primary"
                                                        />
                                                    </TableCell>
                                                    <TableCell align="right">
                                                        {(option.percentage * 100).toFixed(2)}%
                                                    </TableCell>
                                                </TableRow>
                                            ))}
                                        </TableBody>
                                    </Table>
                                </TableContainer>
                            </CardContent>
                        </Card>
                    </Grid>
                </Grid>
            </Paper>
        </Container>
    );
};