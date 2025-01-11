using Microsoft.AspNetCore.Identity;
using OnlinePollSystem.Core.DTOs.Auth;
using OnlinePollSystem.Core.Interfaces;
using OnlinePollSystem.Core.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;

namespace OnlinePollSystem.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            IUserRepository userRepository, 
            IJwtService jwtService,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if user exists
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
                return AuthResultDto.Failed("User already exists");

            // Create new user
            var user = new User
            {
                Email = registerDto.Email,
                Username = registerDto.Username,
                Role = "User"
            };

            // Hash password
            user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

            // Save user
            await _userRepository.CreateAsync(user);

            // Generate token
            var token = _jwtService.GenerateToken(user);

            return AuthResultDto.CreateSuccess(token, new 
            { 
                user.Id, 
                user.Username, 
                user.Email 
            });
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
                return AuthResultDto.CreateFailure("Invalid credentials");

            // Verify password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, loginDto.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                var token = _jwtService.GenerateToken(user);
                return AuthResultDto.Success(token, new 
                { 
                    user.Id, 
                    user.Username, 
                    user.Email 
                });
            }

            return AuthResultDto.CreateFailure("Invalid credentials");
        }
    }
}