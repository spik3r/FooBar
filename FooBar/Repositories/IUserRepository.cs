using System.Collections.Generic;
using System.Threading.Tasks;
using FooBar.Models;

namespace FooBar.Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByDobRangeAsync(DateTime? minDob, DateTime? maxDob);
        Task<User> AddUserAsync(User user);
    }
}

