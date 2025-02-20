using BookAPI.Models;

namespace BookAPI.Service
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
