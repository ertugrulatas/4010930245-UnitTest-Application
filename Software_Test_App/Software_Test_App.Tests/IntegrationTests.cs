using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Software_Test_App.Data;
using Software_Test_App.Models;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Software_Test_App.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            return base.CreateHost(builder);
        }
    }

    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public IntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private HttpClient CreateClient() => _factory.CreateClient();

        private void SeedDatabase(Action<AppDbContext> seedAction)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            seedAction(context);
            context.SaveChanges();
        }

        [Fact]
        public async Task GetUsers_ReturnsAllUsers_IntegrationTest()
        {
            var client = CreateClient();
            
            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 1, Username = "user1", Email = "user1@test.com" });
                context.Users.Add(new User { Id = 2, Username = "user2", Email = "user2@test.com" });
            });

            var response = await client.GetAsync("/api/Users");

            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<User>>();
            Assert.NotNull(users);
            Assert.True(users.Count >= 2);
        }

        [Fact]
        public async Task PostUser_CreatesNewUser_IntegrationTest()
        {
            var client = CreateClient();
            var newUser = new User { Id = 100, Username = "newuser", Email = "new@test.com" };

            var response = await client.PostAsJsonAsync("/api/Users", newUser);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdUser = await response.Content.ReadFromJsonAsync<User>();
            Assert.NotNull(createdUser);
            Assert.Equal("newuser", createdUser.Username);
        }

        [Fact]
        public async Task PutUser_UpdatesExistingUser_IntegrationTest()
        {
            var client = CreateClient();
            
            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 200, Username = "oldname", Email = "old@test.com" });
            });

            var updatedUser = new User { Id = 200, Username = "newname", Email = "new@test.com" };
            var response = await client.PutAsJsonAsync($"/api/Users/200", updatedUser);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_RemovesUser_IntegrationTest()
        {
            var client = CreateClient();
            
            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 300, Username = "deleteuser", Email = "delete@test.com" });
            });

            var response = await client.DeleteAsync($"/api/Users/300");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetEntry_ReturnsNotFound_WhenDoesNotExist_IntegrationTest()
        {
            var client = CreateClient();
            var response = await client.GetAsync("/api/Entries/99999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostEntry_ReturnsBadRequest_WhenInvalidUserId_IntegrationTest()
        {
            var client = CreateClient();
            var newEntry = new Entry { Id = 400, Title = "Test Entry", Content = "Test", UserId = 99999 };

            var response = await client.PostAsJsonAsync("/api/Entries", newEntry);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task User_Entry_Relationship_IntegrationTest()
        {
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 500, Username = "entryuser", Email = "entry@test.com" });
            });

            var entry = new Entry { Id = 500, Title = "User Entry", Content = "Content", UserId = 500 };
            var response = await client.PostAsJsonAsync("/api/Entries", entry);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdEntry = await response.Content.ReadFromJsonAsync<Entry>();
            Assert.NotNull(createdEntry);
            Assert.Equal(500, createdEntry.UserId);
        }

        [Fact]
        public async Task Entry_Review_Relationship_IntegrationTest()
        {
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 600, Username = "reviewuser", Email = "review@test.com" });
                context.Entries.Add(new Entry { Id = 600, Title = "Review Entry", Content = "Content", UserId = 600 });
            });

            var review = new Review { Id = 600, Text = "Great entry!", Rating = 5, EntryId = 600 };
            var response = await client.PostAsJsonAsync("/api/Reviews", review);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdReview = await response.Content.ReadFromJsonAsync<Review>();
            Assert.NotNull(createdReview);
            Assert.Equal(600, createdReview.EntryId);
        }

        [Fact]
        public async Task Entry_Tag_Relationship_IntegrationTest()
        {
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 700, Username = "taguser", Email = "tag@test.com" });
                context.Entries.Add(new Entry { Id = 700, Title = "Tag Entry", Content = "Content", UserId = 700 });
            });

            var tag = new Tag { Id = 700, Name = "CSharp", EntryId = 700 };
            var response = await client.PostAsJsonAsync("/api/Tags", tag);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdTag = await response.Content.ReadFromJsonAsync<Tag>();
            Assert.NotNull(createdTag);
            Assert.Equal(700, createdTag.EntryId);
        }

        [Fact]
        public async Task ComplexCRUD_UserWithEntriesAndReviews_IntegrationTest()
        {
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 800, Username = "complexuser", Email = "complex@test.com" });
            });
            
            var entry = new Entry { Id = 800, Title = "Complex Entry", Content = "Content", UserId = 800 };
            var entryResponse = await client.PostAsJsonAsync("/api/Entries", entry);
            Assert.Equal(HttpStatusCode.Created, entryResponse.StatusCode);

            var review = new Review { Id = 800, Text = "Excellent!", Rating = 5, EntryId = 800 };
            var reviewResponse = await client.PostAsJsonAsync("/api/Reviews", review);
            Assert.Equal(HttpStatusCode.Created, reviewResponse.StatusCode);

            entry.Title = "Updated Complex Entry";
            var updateResponse = await client.PutAsJsonAsync($"/api/Entries/{entry.Id}", entry);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var getReviewResponse = await client.GetAsync($"/api/Reviews/{review.Id}");
            Assert.Equal(HttpStatusCode.OK, getReviewResponse.StatusCode);

            var deleteResponse = await client.DeleteAsync($"/api/Reviews/{review.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task DatabaseCRUD_MultipleEntries_IntegrationTest()
        {
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 900, Username = "multipleuser", Email = "multiple@test.com" });
            });

            for (int i = 1; i <= 3; i++)
            {
                var entry = new Entry 
                { 
                    Id = 900 + i, 
                    Title = $"Entry {i}", 
                    Content = $"Content {i}", 
                    UserId = 900 
                };
                await client.PostAsJsonAsync("/api/Entries", entry);
            }

            var response = await client.GetAsync("/api/Entries");
            response.EnsureSuccessStatusCode();
            var entries = await response.Content.ReadFromJsonAsync<List<Entry>>();
            Assert.NotNull(entries);
            Assert.True(entries.Count >= 3);
        }

        [Fact]
        public async Task Tag_CRUD_Operations_IntegrationTest()
        {
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 1000, Username = "tagcruduser", Email = "tagcrud@test.com" });
                context.Entries.Add(new Entry { Id = 1000, Title = "Tag CRUD Entry", Content = "Content", UserId = 1000 });
            });

            var tag = new Tag { Id = 1000, Name = "Integration", EntryId = 1000 };
            var createResponse = await client.PostAsJsonAsync("/api/Tags", tag);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var getResponse = await client.GetAsync($"/api/Tags/{tag.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            tag.Name = "UpdatedTag";
            var updateResponse = await client.PutAsJsonAsync($"/api/Tags/{tag.Id}", tag);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var deleteResponse = await client.DeleteAsync($"/api/Tags/{tag.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task PostReview_ReturnsBadRequest_WhenInvalidEntryId_IntegrationTest()
        {
            var client = CreateClient();
            var review = new Review { Id = 1100, Text = "Invalid review", Rating = 3, EntryId = 99999 };
            var response = await client.PostAsJsonAsync("/api/Reviews", review);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostTag_ReturnsBadRequest_WhenInvalidEntryId_IntegrationTest()
        {
            var client = CreateClient();
            var tag = new Tag { Id = 1200, Name = "InvalidTag", EntryId = 99999 };
            var response = await client.PostAsJsonAsync("/api/Tags", tag);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenDoesNotExist_IntegrationTest()
        {
            var client = CreateClient();
            var response = await client.GetAsync("/api/Users/88888");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
