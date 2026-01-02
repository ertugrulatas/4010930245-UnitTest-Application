using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Software_Test_App.Controllers;
using Software_Test_App.Data;
using Software_Test_App.Models;
using Xunit;

namespace Software_Test_App.Tests
{
    public class EntriesControllerTests
    {
        private DbContextOptions<AppDbContext> _options;

        public EntriesControllerTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "EntriesTestDb_" + Guid.NewGuid())
                .Options;
        }

        private async Task<AppDbContext> GetDatabaseContext()
        {
            var context = new AppDbContext(_options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task GetEntries_ReturnsAllEntries()
        {
            using var context = await GetDatabaseContext();
            context.Entries.Add(new Entry { Id = 1, Title = "Test 1", UserId = 1 });
            context.Entries.Add(new Entry { Id = 2, Title = "Test 2", UserId = 1 });
            await context.SaveChangesAsync();

            var controller = new EntriesController(context);
            var result = await controller.GetEntries();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var entries = Assert.IsAssignableFrom<IEnumerable<Entry>>(okResult.Value);
            Assert.Equal(2, entries.Count());
        }

        [Fact]
        public async Task GetEntry_ReturnsEntry_WhenExists()
        {
            using var context = await GetDatabaseContext();
            context.Entries.Add(new Entry { Id = 3, Title = "Test 3", UserId = 1 });
            await context.SaveChangesAsync();

            var controller = new EntriesController(context);
            var result = await controller.GetEntry(3);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var entry = Assert.IsType<Entry>(okResult.Value);
            Assert.Equal(3, entry.Id);
        }

        [Fact]
        public async Task GetEntry_ReturnsNotFound_WhenDoesNotExist()
        {
            using var context = await GetDatabaseContext();
            var controller = new EntriesController(context);
            var result = await controller.GetEntry(999);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostEntry_AddsEntry_WhenValid()
        {
            using var context = await GetDatabaseContext();
            context.Users.Add(new User { Id = 1, Username = "testuser" });
            await context.SaveChangesAsync();

            var controller = new EntriesController(context);
            var newEntry = new Entry { Id = 4, Title = "New Entry", UserId = 1 };
            var result = await controller.PostEntry(newEntry);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var entry = Assert.IsType<Entry>(createdAtActionResult.Value);
            Assert.Equal("New Entry", entry.Title);
            Assert.Single(context.Entries);
        }

        [Fact]
        public async Task PostEntry_ReturnsBadRequest_WhenUserDoesNotExist()
        {
            using var context = await GetDatabaseContext();
            var controller = new EntriesController(context);
            var newEntry = new Entry { Id = 5, Title = "New Entry", UserId = 99 };

            var result = await controller.PostEntry(newEntry);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid UserId.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteEntry_RemovesEntry_WhenExists()
        {
            using var context = await GetDatabaseContext();
            context.Entries.Add(new Entry { Id = 6, Title = "Delete Me", UserId = 1 });
            await context.SaveChangesAsync();

            var controller = new EntriesController(context);
            var result = await controller.DeleteEntry(6);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Entries);
        }

        [Fact]
        public async Task DeleteEntry_ReturnsNotFound_WhenDoesNotExist()
        {
            using var context = await GetDatabaseContext();
            var controller = new EntriesController(context);
            var result = await controller.DeleteEntry(999);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
