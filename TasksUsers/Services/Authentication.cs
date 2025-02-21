using UsersTasks.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UsersTasks.Repositories;
using System.Security.Cryptography;

namespace UsersTasks.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken ();
        Task<User?> AuthenticateAsync(string email, string password);
        Task<string> RefreshTokenAsync(string token);
        void StoreRefreshToken(string token, int userId);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private static Dictionary<string, string> _refreshTokens = new();

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.Password != password)
                return null;
            return user;
        }

        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        public async Task<string?> RefreshTokenAsync(string refreshToken)
        {
            var user = _refreshTokens.ContainsKey(refreshToken) ? await _userRepository.GetByIdAsync(int.Parse(_refreshTokens[refreshToken])) : null;
            if (user == null) return null;

            var newAccessToken = GenerateJwtToken(user);
            return newAccessToken;
        }

        public void StoreRefreshToken(string refreshToken, int userId)
        {
            _refreshTokens[refreshToken] = userId.ToString();
        }
    }
}
