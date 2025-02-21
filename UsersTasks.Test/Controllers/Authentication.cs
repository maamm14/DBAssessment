using Microsoft.AspNetCore.Mvc;
using Moq;
using UsersTasks.Controllers;
using UsersTasks.Models;
using UsersTasks.Models.DTO;
using UsersTasks.Services;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UsersTasks.Tests.Controllers
{
    [TestClass]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _authServiceMock = null!;
        private AuthController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [TestMethod]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new Login { Email = "invalid@example.com", Password = "wrongpassword" };
            _authServiceMock.Setup(s => s.AuthenticateAsync(loginDto.Email, loginDto.Password))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task Login_ValidCredentials_ReturnsOkWithTokens()
        {
            // Arrange
            var loginDto = new Login { Email = "user@example.com", Password = "correctpassword" };
            var user = new User { Id = 1, Email = "user@example.com" };

            _authServiceMock.Setup(s => s.AuthenticateAsync(loginDto.Email, loginDto.Password))
                .ReturnsAsync(user);
            _authServiceMock.Setup(s => s.GenerateJwtToken(user)).Returns("fake-jwt-token");
            _authServiceMock.Setup(s => s.GenerateRefreshToken()).Returns("fake-refresh-token");

            // Act
            var result = await _controller.Login(loginDto) as OkObjectResult;
            var value = result?.Value as AuthResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(value);
            Assert.AreEqual("fake-jwt-token", value.AccessToken);
            Assert.AreEqual("fake-refresh-token", value.RefreshToken);

            _authServiceMock.Verify(s => s.StoreRefreshToken("fake-refresh-token", user.Id), Times.Once);
        }

        [TestMethod]
        public async Task Refresh_InvalidToken_ReturnsUnauthorized()
        {
            // Arrange
            var refreshTokenDto = new RefreshToken { Token = "invalid-token" };
            _authServiceMock.Setup(s => s.RefreshTokenAsync(refreshTokenDto.Token))
                .ReturnsAsync((string?)null);

            // Act
            var result = await _controller.Refresh(refreshTokenDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task Refresh_ValidToken_ReturnsNewAccessToken()
        {
            // Arrange
            var refreshTokenDto = new RefreshToken { Token = "valid-token" };
            _authServiceMock.Setup(s => s.RefreshTokenAsync(refreshTokenDto.Token))
                .ReturnsAsync("new-fake-access-token");

            // Act
            var result = await _controller.Refresh(refreshTokenDto) as OkObjectResult;
            var value = result?.Value as AuthResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(value);
            Assert.AreEqual("new-fake-access-token", value.AccessToken);
        }
    }
}
