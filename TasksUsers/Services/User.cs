using UsersTasks.Models;
using UsersTasks.Repositories;
using UsersTasks.Models.DTO;

namespace UsersTasks.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task CreateUserAsync(CreateUser createUser);
        Task UpdateUserAsync(UpdateUser updateUser);
        Task DeleteUserAsync(int id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task CreateUserAsync(CreateUser createUser)
        {
            var user = new User
            {
                Username = createUser.Username,
                Email = createUser.Email,
                Password = createUser.Password,
            };
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(UpdateUser updateUser)
        {
            var user = new User
            {
                Id = updateUser.Id,
                Username = updateUser.Username,
                Email = updateUser.Email,
                Password = updateUser.Password,
            };
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }

}
