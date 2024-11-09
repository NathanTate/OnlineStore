using API.Models;
using API.Models.DTO.UserDTO;

namespace API.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public Task<TokenDto> GenerateToken(ApplicationUser user, bool populateExp = false);
    }
}
