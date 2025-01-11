import React, { useState, useEffect } from 'react';
import {
    List,
    ListItem,
    ListItemText,
    Container,
    Typography,
    Button,
    Box
} from '@mui/material';
import { pollService } from '../../services/pollService';
import { Poll } from '../../models/Poll';

export const PollList: React.FC = () => {
    const [polls, setPolls] = useState<Poll[]>([]);
    const [page, setPage] = useState(1);

    useEffect(() => {
        const fetchPolls = async () => {
            try {
                const data = await pollService.getActivePolls(page);
                setPolls(data.items);
            } catch (error) {
                console.error('Failed to fetch polls', error);
            }
        };

        fetchPolls();
    }, [page]);

    return (
        <Container>
            <Typography variant="h4" sx={{ my: 2 }}>
                Active Polls
            </Typography>
            <List>
                {polls.map(poll => (
                    <ListItem key={poll.id}>
                        <ListItemText
                            primary={poll.title}
                            secondary={poll.description}
                        />
                        <Button
                            variant="contained"
                            color="primary"
                            href={`/poll/${poll.id}`}
                        >
                            View Poll
                        </Button>
                    </ListItem>
                ))}
            </List>
            <Box sx={{ display: 'flex', justifyContent: 'center', my: 2 }}>
                <Button
                    onClick={() => setPage(prev => prev - 1)}
                    disabled={page === 1}
                >
                    Previous
                </Button>
                <Button onClick={() => setPage(prev => prev + 1)}>
                    Next
                </Button>
            </Box>
        </Container>
    );
};