import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../Auth/AuthContent';
import { LoadingSpinner } from './LoadingSpinner';

interface PrivateRouteProps {
    children: React.ReactNode;
}

export const PrivateRoute: React.FC<PrivateRouteProps> = ({ children }) => {
    const { isAuthenticated, loading } = useAuth();

    if (loading) {
        return <LoadingSpinner />;
    }

    return isAuthenticated
        ? <>{children}</>
        : <Navigate to="/login" replace />;
};