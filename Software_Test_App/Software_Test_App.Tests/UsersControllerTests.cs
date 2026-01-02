using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Software_Test_App.Controllers;
using Software_Test_App.Data;
using Software_Test_App.Models;
using Xunit;

namespace Software_Test_App.Tests
{
    public class UsersControllerTests
    {
        private DbContextOptions<AppDbContext> _options;

        public UsersControllerTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UsersTestDb_" + Guid.NewGuid())
                .Options;
        }

        private async Task<AppDbContext> GetDatabaseContext()
        {
            var context = new AppDbContext(_options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task GetUsers_ReturnsAllUsers()
        {
            // Arrange
            using var context = await GetDatabaseContext();
            context.Users.Add(new User { Id = 1, Username = "User 1" });
            context.Users.Add(new User { Id = 2, Username = "User 2" });
            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            // Act
            var result = await controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task GetUser_ReturnsUser_WhenExists()
        {
            // Arrange
            using var context = await GetDatabaseContext();
            context.Users.Add(new User { Id = 3, Username = "User 3" });
            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            // Act
            var result = await controller.GetUser(3);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var user = Assert.IsType<User>(okResult.Value);
            Assert.Equal(3, user.Id);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenDoesNotExist()
        {
            // Arrange
            using var context = await GetDatabaseContext();
            var controller = new UsersController(context);

            // Act
            var result = await controller.GetUser(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostUser_AddsUser()
        {
            // Arrange
            using var context = await GetDatabaseContext();
            var controller = new UsersController(context);
            var newUser = new User { Id = 4, Username = "New User" };

            // Act
            var result = await controller.PostUser(newUser);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var user = Assert.IsType<User>(createdAtActionResult.Value);
            Assert.Equal("New User", user.Username);
            Assert.Single(context.Users);
        }

        [Fact]
        public async Task DeleteUser_RemovesUser_WhenExists()
        {
            // Arrange
            using var context = await GetDatabaseContext();
            context.Users.Add(new User { Id = 5, Username = "Delete Me" });
            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            // Act
            var result = await controller.DeleteUser(5);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Users);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenDoesNotExist()
        {
            // Arrange
            using var context = await GetDatabaseContext();
            var controller = new UsersController(context);

            // Act
            var result = await controller.DeleteUser(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
