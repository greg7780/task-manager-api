using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Handlers;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public class AuthenticationService
    {
        private readonly TaskManagerContext _taskManagerContext;
        private readonly IConfiguration _configuration;

        public AuthenticationService(TaskManagerContext taskManagerContext, IConfiguration configuration)
        {
            _taskManagerContext = taskManagerContext;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO request)
        {
            var user = await _taskManagerContext.Users
                .FirstOrDefaultAsync(i => i.Username == request.Username);

            if (user == null || !PasswordHashHandler.VerifyHashedPassword(user.Password, request.Password))
                return new LoginResponseDTO { Message = "Invalid username or password" };

            var accessToken = JwtTokenHandler.GenerateToken(user, _configuration);

            var refreshToken = GenerateRefreshToken();
            refreshToken.UserId = user.Id;

            await _taskManagerContext.RefreshTokens.AddAsync(refreshToken);
            await _taskManagerContext.SaveChangesAsync();

            return new LoginResponseDTO
            {
                Username = request.Username,
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Token,
                ExpiresIn = (int)accessToken.ExpiresAt.Subtract(DateTime.UtcNow).TotalSeconds,
                Message = "Login successful"
            };
        }

        public async Task<LoginResponseDTO> RefreshToken(string token)
        {
            var refreshToken = await _taskManagerContext.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token);

            if (refreshToken == null || refreshToken.User == null || refreshToken.Expires < DateTime.UtcNow)
                return new LoginResponseDTO { Message = "Invalid or expired refresh token" };

            var newAccessToken = JwtTokenHandler.GenerateToken(refreshToken.User, _configuration);
            return new LoginResponseDTO
            {
                Username = refreshToken.User.Username,
                AccessToken = newAccessToken.Token,
                RefreshToken = token,
                Message = "Token refreshed"
            };
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshTokenValidityDays = _configuration.GetValue<int>("JwtConfig:RefreshTokenValidityDays");
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return new RefreshToken() 
            { 
                Token = Convert.ToBase64String(randomBytes), 
                Expires = DateTime.UtcNow.AddDays(refreshTokenValidityDays), 
                UserId = 0
            };
        }
    }
}
