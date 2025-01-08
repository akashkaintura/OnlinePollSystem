using Microsoft.EntityFrameworkCore;
using OnlinePollSystem.Core.Interfaces;
using OnlinePollSystem.Core.Models;
using OnlinePollSystem.Infrastructure.Data;

namespace OnlinePollSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> CheckPasswordAsync(string email, string passwordHash)
        {
            var user = await GetByEmailAsync(email);
            return user != null && user.PasswordHash == passwordHash;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}