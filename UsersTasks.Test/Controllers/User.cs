using Moq;
using UsersTasks.Controllers;
using UsersTasks.Models;
using UsersTasks.Models.DTO;
using UsersTasks.Services;

namespace UsersTasks.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private UserController _controller;

        [TestInitialize]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [TestMethod]
        public async Task GetAllUsers_ReturnsUsers()
        {
            // Arrange
            var users = new List<User> { new User { Id = 1, Username = "testUser", Email = "user@example.com", Password = "hashedpassword" } };
            _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetUserById_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testUser", Email = "user@example.com", Password = "hashedpassword" };
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task Post_CreatesUser()
        {
            // Arrange
            var createUser = new CreateUser { Username = "newuser", Email = "newuser@example.com", Password = "securepassword" };
            _userServiceMock.Setup(s => s.CreateUserAsync(createUser)).Returns(Task.CompletedTask);

            // Act
            await _controller.Post(createUser);

            // Assert
            _userServiceMock.Verify(s => s.CreateUserAsync(createUser), Times.Once);
        }

        [TestMethod]
        public async Task Put_UpdatesUser()
        {
            // Arrange
            var updateUser = new UpdateUser { Id = 1, Username = "updateduser", Email = "updated@example.com", Password = "updatedpassword" };
            _userServiceMock.Setup(s => s.UpdateUserAsync(updateUser)).Returns(Task.CompletedTask);

            // Act
            await _controller.Put(1, updateUser);

            // Assert
            _userServiceMock.Verify(s => s.UpdateUserAsync(updateUser), Times.Once);
        }

        [TestMethod]
        public async Task Delete_DeletesUser()
        {
            // Arrange
            _userServiceMock.Setup(s => s.DeleteUserAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _controller.Delete(1);

            // Assert
            _userServiceMock.Verify(s => s.DeleteUserAsync(1), Times.Once);
        }
    }
}
