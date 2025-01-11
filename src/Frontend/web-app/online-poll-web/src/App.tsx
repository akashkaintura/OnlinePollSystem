// src/App.tsx
import React from 'react';
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate
} from 'react-router-dom';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';

// Auth Components
import { Login } from './components/Auth/Login';
import { Register } from './components/Auth/Register';

// Poll Components
import { PollList } from './components/Polls/PollList';
import { PollCreate } from './components/Polls/PollCreate';
import { PollDetails } from './components/Polls/PollDetails';
import { PollResults } from './components/Polls/PollResult';

// User Components
import { UserProfile } from './components/User/UserProfile';
import { Dashboard } from './components/Dashboard/Dashboard';

// Common Components
import { Header } from './components/Common/Header';
import { PrivateRoute } from './components/Common/PrivateRoute';
import { AuthProvider } from './components/Auth/AuthContent';

// Theme Configuration
const theme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
  },
  typography: {
    fontFamily: 'Roboto, Arial, sans-serif',
  },
});

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <AuthProvider>
        <Router>
          <Header />
          <Routes>
            {/* Public Routes */}
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />

            {/* Private Routes */}
            <Route
              path="/dashboard"
              element={
                <PrivateRoute>
                  <Dashboard />
                </PrivateRoute>
              }
            />
            <Route
              path="/polls"
              element={
                <PrivateRoute>
                  <PollList />
                </PrivateRoute>
              }
            />
            <Route
              path="/create-poll"
              element={
                <PrivateRoute>
                  <PollCreate />
                </PrivateRoute>
              }
            />
            <Route
              path="/poll/:pollId"
              element={
                <PrivateRoute>
                  <PollDetails />
                </PrivateRoute>
              }
            />
            <Route
              path="/poll/:pollId/results"
              element={
                <PrivateRoute>
                  <PollResults />
                </PrivateRoute>
              }
            />
            <Route
              path="/profile"
              element={
                <PrivateRoute>
                  <UserProfile />
                </PrivateRoute>
              }
            />

            {/* Redirect */}
            <Route path="/" element={<Navigate to="/dashboard" replace />} />
            <Route path="*" element={<Navigate to="/dashboard" replace />} />
          </Routes>
        </Router>
      </AuthProvider>
    </ThemeProvider>
  );
}
export default App;