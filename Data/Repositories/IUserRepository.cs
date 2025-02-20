using BookAPI.Models;

namespace BookAPI.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user);
        Task<bool> UserExistsAsync(string username);
    }
}
