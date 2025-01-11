import axios from 'axios';

const API_URL = 'http://localhost:5000/api/poll';

export const pollService = {
    getActivePolls: async (page = 1, pageSize = 10) => {
        const response = await axios.get(`${API_URL}?page=${page}&pageSize=${pageSize}`);
        return response.data;
    },
    createPoll: async (pollData: any) => {
        const response = await axios.post(API_URL, pollData);
        return response.data;
    },
    votePoll: async (pollId: number, optionId: number) => {
        const response = await axios.post(`${API_URL}/${pollId}/vote`, { optionId });
        return response.data;
    }
};