using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FooBar.Models;
using Microsoft.EntityFrameworkCore;

namespace FooBar.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetUsersByDobRangeAsync(DateTime? minDob, DateTime? maxDob)
        {
            IQueryable<User> query = _context.Users;

            if (minDob.HasValue)
            {
                query = query.Where(u => u.Dob >= minDob.Value);
            }

            if (maxDob.HasValue)
            {
                query = query.Where(u => u.Dob <= maxDob.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}

