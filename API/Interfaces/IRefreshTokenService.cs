using System.Security.Claims;
using FluentResults;

namespace API.Interfaces;

public interface IRefreshTokenService
{
  string GenerateRefreshToken();
  Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
}