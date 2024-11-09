using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Interfaces;
using API.Utility;
using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly JwtOptions _jwtOptions;
    public RefreshTokenService(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }
    public string GenerateRefreshToken()
    {
        byte[] buffer = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(buffer);

            return Convert.ToBase64String(buffer);
        }
    }

    public Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidAudience = _jwtOptions.Audience,
            ValidIssuer = _jwtOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        ClaimsPrincipal principal;
        try
        {
            principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        }
        catch (ArgumentException ex)
        {
            return Result.Fail("Invalid token format");
        }
        catch (Exception ex)
        {
            throw;
        }

        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
        {
            return Result.Fail("Invalid Token");
        }

        return principal;
    }

}