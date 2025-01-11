import axios from 'axios';
import { AuthResponse, User } from '../models/User';
const API_URL = 'http://localhost:5000/api/auth';
export const authService = {
    login: async (email: string, password: string): Promise<AuthResponse> => {
        const response = await axios.post<AuthResponse>(`${API_URL}/login`, {
            email,
            password
        });
        return response.data;
    },

    register: async (
        username: string,
        email: string,
        password: string
    ): Promise<AuthResponse> => {
        const response = await axios.post<AuthResponse>(`${API_URL}/register`, {
            username,
            email,
            password
        });
        return response.data;
    },

    getCurrentUser: async (): Promise<User> => {
        const response = await axios.get<User>(`${API_URL}/me`);
        return response.data;
    }
};
