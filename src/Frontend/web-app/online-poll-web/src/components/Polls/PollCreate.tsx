import React, { useState } from 'react';
import {
    Container,
    Typography,
    TextField,
    Button,
    Box,
    IconButton,
    Grid,
    Paper,
    Alert
} from '@mui/material';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import DeleteIcon from '@mui/icons-material/Delete';
import { pollService } from '../../services/pollService';
import { useNavigate } from 'react-router-dom';

export const PollCreate: React.FC = () => {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [options, setOptions] = useState<string[]>(['', '']);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleAddOption = () => {
        if (options.length < 10) {
            setOptions([...options, '']);
        }
    };

    const handleRemoveOption = (index: number) => {
        if (options.length > 2) {
            const newOptions = [...options];
            newOptions.splice(index, 1);
            setOptions(newOptions);
        }
    };

    const handleOptionChange = (index: number, value: string) => {
        const newOptions = [...options];
        newOptions[index] = value;
        setOptions(newOptions);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setLoading(true);

        // Validate inputs
        if (!title.trim()) {
            setError('Poll title is required');
            setLoading(false);
            return;
        }

        const validOptions = options.filter(option => option.trim() !== '');
        if (validOptions.length < 2) {
            setError('At least two options are required');
            setLoading(false);
            return;
        }

        try {
            const pollData = {
                title,
                description,
                options: validOptions.map(text => ({
                    optionText: text,
                    id: 0,
                    pollId: 0,
                    voteCount: 0
                })),
                startDate: new Date().toISOString(),
                endDate: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString(), // 7 days from now
                isActive: true
            };

            const createdPoll = await pollService.createPoll(pollData);
            navigate(`/poll/${createdPoll.id}`);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Failed to create poll');
        } finally {
            setLoading(false);
        }
    };

    return (
        <Container maxWidth="md" sx={{ mt: 4 }}>
            <Paper elevation={3} sx={{ p: 4 }}>
                <Typography variant="h4" gutterBottom>
                    Create New Poll
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

                <form onSubmit={handleSubmit}>
                    <TextField
                        fullWidth
                        label="Poll Title"
                        variant="outlined"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        margin="normal"
                        required
                    />

                    <TextField
                        fullWidth
                        label="Description (Optional)"
                        variant="outlined"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        margin="normal"
                        multiline


                        rows={4}
                    />

                    <Typography variant="h6" sx={{ mt: 2, mb: 2 }}>
                        Poll Options
                    </Typography>

                    {options.map((option, index) => (
                        <Grid container spacing={2} key={index} sx={{ mb: 1 }}>
                            <Grid item xs={10}>
                                <TextField
                                    fullWidth
                                    label={`Option ${index + 1}`}
                                    variant="outlined"
                                    value={option}
                                    onChange={(e) => handleOptionChange(index, e.target.value)}
                                    required
                                />
                            </Grid>
                            <Grid item xs={2}>
                                {options.length > 2 && (
                                    <IconButton
                                        color="error"
                                        onClick={() => handleRemoveOption(index)}
                                        disabled={loading}
                                    >
                                        <DeleteIcon />
                                    </IconButton>
                                )}
                            </Grid>
                        </Grid>
                    ))}

                    <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 2 }}>
                        <Button
                            variant="outlined"
                            startIcon={<AddCircleOutlineIcon />}
                            onClick={handleAddOption}
                            disabled={options.length >= 10 || loading}
                        >
                            Add Option
                        </Button>
                        <Typography variant="caption" color="textSecondary">
                            {options.length}/10 options
                        </Typography>
                    </Box>

                    <Box sx={{ mt: 3, display: 'flex', justifyContent: 'space-between' }}>
                        <Button
                            variant="contained"
                            color="primary"
                            type="submit"
                            disabled={loading}
                        >
                            {loading ? 'Creating Poll...' : 'Create Poll'}
                        </Button>
                        <Button
                            variant="outlined"
                            color="secondary"
                            onClick={() => navigate('/polls')}
                            disabled={loading}
                        >
                            Cancel
                        </Button>
                    </Box>
                </form>
            </Paper>
        </Container>
    );
};