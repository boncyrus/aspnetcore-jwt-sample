using AspNetCoreJwt.Data;
using AspNetCoreJwt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreJwt.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IEncryptionService _encryptionService;

        public UserService(ApplicationDbContext dbContext, ITokenService tokenService, IOptions<AppSettings> appSettings, IEncryptionService encryptionService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _appSettings = appSettings;
            _encryptionService = encryptionService;
        }

        private IList<Claim> CreateClaims(UserEntity user)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email)
            };
        }

        public async Task<SignInResponse> SignIn(SignInRequest request)
        {
            var encryptedPassword = _encryptionService.Encrypt(request.Password);
            var userToSignIn = _dbContext.Users.FirstOrDefault(x => x.Email == request.Username && x.Password == encryptedPassword);

            if (userToSignIn == null)
            {
                throw new Exception("Invalid username or passworrd");
            }

            var refreshToken = _tokenService.GenerateRefreshToken();
            userToSignIn.RefreshTokens.Add(new TokenEntity()
            {
                Token = refreshToken
            });

            await _dbContext.SaveChangesAsync();
            var token = _tokenService.GenerateToken(_appSettings.Value.JwtSettings.Secret, CreateClaims(userToSignIn), _appSettings.Value.JwtSettings.Expiration);
            var response = new SignInResponse()
            {
                Username = request.Username,
                Token = token.Token,
                RefreshToken = refreshToken,
                Expiration = token.Expiration
            };

            return response;
        }

        public async Task<SignUpResponse> SignUp(SignUpRequest request)
        {
            var existingUser = _dbContext.Users.FirstOrDefault(x => x.Email == request.Username);

            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            var refreshToken = new TokenEntity()
            {
                Token = _tokenService.GenerateRefreshToken(),
            };

            var tokens = new List<TokenEntity>()
            {
                refreshToken
            };

            var userToRegister = new UserEntity()
            {
                Email = request.Username,
                Password = _encryptionService.Encrypt(request.Password),
                RefreshTokens = tokens
            };

            var token = _tokenService.GenerateToken(_appSettings.Value.JwtSettings.Secret, CreateClaims(userToRegister), _appSettings.Value.JwtSettings.Expiration);
            SignUpResponse response = new SignUpResponse()
            {
                Username = request.Username,
                RefreshToken = refreshToken.Token,
                Token = token.Token,
                Expiration = token.Expiration
            };

            _dbContext.Users.Add(userToRegister);
            await _dbContext.SaveChangesAsync();

            return response;
        }

        public Task<GetUsersResponse> GetUsers()
        {
            var users = _dbContext.Users.Include(x => x.RefreshTokens).AsNoTracking();
            return Task.FromResult(new GetUsersResponse()
            {
                Users = users
            });
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            var tokenEntity = _dbContext.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefault(x => x.Token == request.RefreshToken);

            if (tokenEntity == null)
            {
                throw new Exception("Refresh token is invalid.");
            }

            var appSettings = _appSettings.Value;
            var refreshToken = _tokenService.GenerateRefreshToken();

            var token = _tokenService.GenerateToken(appSettings.JwtSettings.Secret, CreateClaims(tokenEntity.User), appSettings.JwtSettings.Expiration);
            var response = new RefreshTokenResponse()
            {
                Username = tokenEntity.User.Email,
                RefreshToken = refreshToken,
                Token = token.Token,
                Expiration = token.Expiration
            };

            tokenEntity.Token = refreshToken;
            _dbContext.RefreshTokens.Update(tokenEntity);
            await _dbContext.SaveChangesAsync();

            return response;
        }
    }
}