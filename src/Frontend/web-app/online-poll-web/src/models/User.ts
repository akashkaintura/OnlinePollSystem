export interface User {
    id: number;
    username: string;
    email: string;
    createdAt: string;
}

export interface AuthResponse {
    success: boolean;
    token: string;
    user: User;
    message?: string;
}