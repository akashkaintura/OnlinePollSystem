import axios from 'axios';
import { Poll, PollResult, PaginationResult } from '../models/Poll';

const API_URL = 'http://localhost:5000/api/poll';

export const pollService = {
    getActivePolls: async (page = 1, pageSize = 10): Promise<PaginationResult<Poll>> => {
        const response = await axios.get(`${API_URL}?page=${page}&pageSize=${pageSize}`);
        return response.data;
    },

    getPollDetails: async (pollId: number): Promise<Poll> => {
        const response = await axios.get(`${API_URL}/${pollId}`);
        return response.data;
    },

    createPoll: async (pollData: Partial<Poll>): Promise<Poll> => {
        const response = await axios.post(API_URL, pollData);
        return response.data;
    },

    votePoll: async (pollId: number, optionId: number): Promise<void> => {
        await axios.post(`${API_URL}/${pollId}/vote`, { optionId });
    },

    getPollResults: async (pollId: number): Promise<PollResult> => {
        const response = await axios.get(`${API_URL}/${pollId}/results`);
        return response.data;
    },

    getUserCreatedPolls: async (): Promise<Poll[]> => {
        const response = await axios.get('/poll/user/created');
        return response.data;
    },
};