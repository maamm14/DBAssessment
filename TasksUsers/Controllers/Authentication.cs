using Microsoft.AspNetCore.Mvc;
using UsersTasks.Services;
using UsersTasks.Models.DTO;
using System.Threading.Tasks;

namespace UsersTasks.Controllers
{   
    [Route("api/authentication")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = await _authService.AuthenticateAsync(login.Email, login.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var accessToken = _authService.GenerateJwtToken(user);
            var refreshToken = _authService.GenerateRefreshToken();
            _authService.StoreRefreshToken(refreshToken, user.Id);

            return Ok(new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshToken refreshToken)
        {
            var newAccessToken = await _authService.RefreshTokenAsync(refreshToken.Token);
            if (newAccessToken == null)
                return Unauthorized(new { message = "Invalid refresh token" });

            return Ok(new AuthResponse { AccessToken = newAccessToken });
        }
    }
}
