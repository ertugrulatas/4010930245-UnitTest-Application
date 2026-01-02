using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Software_Test_App.Controllers;
using Software_Test_App.Data;
using Software_Test_App.Models;
using Xunit;

namespace Software_Test_App.Tests
{
    public class ReviewsControllerTests
    {
        private DbContextOptions<AppDbContext> _options;

        public ReviewsControllerTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ReviewsTestDb_" + Guid.NewGuid())
                .Options;
        }

        private async Task<AppDbContext> GetDatabaseContext()
        {
            var context = new AppDbContext(_options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task GetReviews_ReturnsAllReviews()
        {
            using var context = await GetDatabaseContext();
            context.Reviews.Add(new Review { Id = 1, Text = "Review 1", EntryId = 1 });
            context.Reviews.Add(new Review { Id = 2, Text = "Review 2", EntryId = 1 });
            await context.SaveChangesAsync();

            var controller = new ReviewsController(context);
            var result = await controller.GetReviews();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var reviews = Assert.IsAssignableFrom<IEnumerable<Review>>(okResult.Value);
            Assert.Equal(2, reviews.Count());
        }

        [Fact]
        public async Task PostReview_AddsReview_WhenValid()
        {
            using var context = await GetDatabaseContext();
            context.Entries.Add(new Entry { Id = 1, Title = "Entry 1", UserId = 1 });
            await context.SaveChangesAsync();

            var controller = new ReviewsController(context);
            var newReview = new Review { Id = 3, Text = "New Review", EntryId = 1 };
            var result = await controller.PostReview(newReview);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var review = Assert.IsType<Review>(createdAtActionResult.Value);
            Assert.Equal("New Review", review.Text);
            Assert.Single(context.Reviews);
        }

        [Fact]
        public async Task PostReview_ReturnsBadRequest_WhenEntryDoesNotExist()
        {
            using var context = await GetDatabaseContext();
            var controller = new ReviewsController(context);
            var newReview = new Review { Id = 4, Text = "New Review", EntryId = 99 };

            var result = await controller.PostReview(newReview);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid EntryId.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteReview_RemovesReview()
        {
            using var context = await GetDatabaseContext();
            context.Reviews.Add(new Review { Id = 5, Text = "Delete Me", EntryId = 1 });
            await context.SaveChangesAsync();

            var controller = new ReviewsController(context);
            var result = await controller.DeleteReview(5);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Reviews);
        }
    }
}
