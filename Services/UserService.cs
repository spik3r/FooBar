using System.Collections.Generic;
using System.Threading.Tasks;
using FooBar.Data;
using FooBar.Models;

namespace FooBar.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return _userRepository.GetAllUsersAsync();
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return _userRepository.GetUserByIdAsync(id);
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _userRepository.GetUserByEmailAsync(email);
        }

        public Task<IEnumerable<User>> GetUsersByDobRangeAsync(DateTime? minDob, DateTime? maxDob)
        {
            return _userRepository.GetUsersByDobRangeAsync(minDob, maxDob);
        }

        public Task<User> AddUserAsync(User user)
        {
            return _userRepository.AddUserAsync(user);
        }
    }
}

