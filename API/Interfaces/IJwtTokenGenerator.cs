using API.Models;

namespace API.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public Task<string> GenerateToken(ApplicationUser user);
    }
}
