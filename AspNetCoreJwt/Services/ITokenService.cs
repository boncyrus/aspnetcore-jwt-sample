using AspNetCoreJwt.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNetCoreJwt.Services
{
    public interface ITokenService
    {
        GenerateTokenResult GenerateToken(string appSecret, IEnumerable<Claim> claims, double expirationInMinutes);

        string GenerateRefreshToken();
    }
}