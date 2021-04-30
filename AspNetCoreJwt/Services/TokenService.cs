using AspNetCoreJwt.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCoreJwt.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public TokenService(IOptions<AppSettings> appSettings, TokenValidationParameters tokenValidationParameters)
        {
            _appSettings = appSettings;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public GenerateTokenResult GenerateToken(string appSecret, IEnumerable<Claim> claims, double expirationInMinutes)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSecret);

            var claimsDictionary = new Dictionary<string, object>();

            foreach (var claim in claims)
            {
                claimsDictionary.Add(claim.Type, claim.Value);
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _appSettings.Value.JwtSettings.Audience,
                Issuer = _appSettings.Value.JwtSettings.Issuer,
                Subject = new ClaimsIdentity(claims),
                Claims = claimsDictionary,
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new GenerateTokenResult()
            {
                Expiration = tokenDescriptor.Expires.Value,
                Token = tokenHandler.WriteToken(token),
            };
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                var isValidAlgo =
                    validatedToken is JwtSecurityToken jwtSecurityToken
                    && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (isValidAlgo)
                {
                    return principal;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}