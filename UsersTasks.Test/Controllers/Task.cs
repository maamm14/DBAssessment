using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersTasks.Controllers;
using UsersTasks.Models;
using UsersTasks.Models.DTO;
using UsersTasks.Services;

namespace UsersTasks.Tests.Controllers
{
    [TestClass]
    public class TasksControllerTests
    {
        private Mock<ITaskService> _taskServiceMock;
        private TasksController _controller;

        [TestInitialize]
        public void Setup()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _controller = new TasksController(_taskServiceMock.Object);
        }

        [TestMethod]
        public async Task Get_ReturnsAllTasks()
        {
            // Arrange
            var tasks = new List<TaskItem> { new TaskItem { Id = 1, Title = "Test Task", Description = "Desc", AssigneeId = 2, DueDate = DateTime.Now } };
            _taskServiceMock.Setup(s => s.GetAllTasksAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task Get_ById_ReturnsTask()
        {
            // Arrange
            var task = new TaskItem { Id = 1, Title = "Test Task", Description = "Desc", AssigneeId = 2, DueDate = DateTime.Now };
            _taskServiceMock.Setup(s => s.GetTaskByIdAsync(1)).ReturnsAsync(task);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task Post_CreatesTask()
        {
            // Arrange
            var createTask = new CreateTask { Title = "New Task", Description = "New Desc", AssigneeId = 3 };
            _taskServiceMock.Setup(s => s.CreateTaskAsync(createTask)).Returns(Task.CompletedTask);

            // Act
            await _controller.Post(createTask);

            // Assert
            _taskServiceMock.Verify(s => s.CreateTaskAsync(createTask), Times.Once);
        }

        [TestMethod]
        public async Task Put_UpdatesTask()
        {
            // Arrange
            var updateTask = new UpdateTask { Id = 1, Title = "Updated Task", Description = "Updated Desc", AssigneeId = 2 };
            _taskServiceMock.Setup(s => s.UpdateTaskAsync(updateTask)).Returns(Task.CompletedTask);

            // Act
            await _controller.Put(1, updateTask);

            // Assert
            _taskServiceMock.Verify(s => s.UpdateTaskAsync(updateTask), Times.Once);
        }

        [TestMethod]
        public async Task Delete_DeletesTask()
        {
            // Arrange
            _taskServiceMock.Setup(s => s.DeleteTaskAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _controller.Delete(1);

            // Assert
            _taskServiceMock.Verify(s => s.DeleteTaskAsync(1), Times.Once);
        }
    }
}
