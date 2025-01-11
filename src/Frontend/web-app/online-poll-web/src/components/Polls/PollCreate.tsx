import React, { useState } from 'react';
import {
    TextField,
    Button,
    Container,
    Typography,
    Box,
    IconButton
} from '@mui/material';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import DeleteIcon from '@mui/icons-material/Delete';
import { pollService } from '../../services/pollService';
import { useNavigate } from 'react-router-dom';

export const PollCreate: React.FC = () => {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [options, setOptions] = useState<string[]>(['', '']);
    const navigate = useNavigate();

    const handleAddOption = () => {
        setOptions([...options, '']);
    };

    const handleRemoveOption = (index: number) => {
        const newOptions = options.filter((_, i) => i !== index);
        setOptions(newOptions);
    };

    const handleOptionChange = (index: number, value: string) => {
        const newOptions = [...options];
        newOptions[index] = value;
        setOptions(newOptions);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await pollService.createPoll({
                title,
                description,
                startDate: new Date().toISOString(),
                endDate: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString(), // 7 days from now
                options: options.map(text => ({ id: 0, pollId: 0, optionText: text, voteCount: 0 }))
            });
            navigate('/polls');
        } catch (error) {
            console.error('Failed to create poll', error);
        }
    };

    return (
        <Container maxWidth="md">
            <Typography variant="h4" sx={{ my: 2 }}>
                Create New Poll
            </Typography>
            <Box component="form" onSubmit={handleSubmit}>
                <TextField
                    fullWidth
                    label="Poll Title"
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                    required
                    sx={{ mb: 2 }}
                />
                <TextField
                    fullWidth
                    label="Description"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    multiline
                    rows={4}
                    sx={{ mb: 2 }}
                />

                <Typography variant="h6" sx={{ mb: 2 }}>
                    Poll Options
                </Typography>
                {options.map((option, index) => (
                    <Box key={index} sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                        <TextField
                            fullWidth
                            label={`Option ${index + 1}`}
                            value={option}
                            onChange={(e) => handleOptionChange(index, e.target.value)}
                            required
                        />
                        {options.length > 2 && (
                            <IconButton onClick={() => handleRemoveOption(index)}>
                                <DeleteIcon />
                            </IconButton>
                        )}
                    </Box>
                ))}

                <Button
                    startIcon={<AddCircleOutlineIcon />}
                    onClick={handleAddOption}
                    sx={{ mb: 2 }}
                >
                    Add Option
                </Button>

                <Button
                    type="submit"
                    variant="contained"
                    fullWidth
                >
                    Create Poll
                </Button>
            </Box>
        </Container>
    );
};