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

        // Test 1: GET endpoint - Tüm kullanıcıları getir
        [Fact]
        public async Task GetUsers_ReturnsAllUsers_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();
            
            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 1, Username = "user1", Email = "user1@test.com" });
                context.Users.Add(new User { Id = 2, Username = "user2", Email = "user2@test.com" });
            });

            // Act
            var response = await client.GetAsync("/api/Users");

            // Assert
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<User>>();
            Assert.NotNull(users);
            Assert.True(users.Count >= 2);
        }

        // Test 2: POST endpoint - Yeni kullanıcı oluştur
        [Fact]
        public async Task PostUser_CreatesNewUser_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();
            var newUser = new User { Id = 100, Username = "newuser", Email = "new@test.com" };

            // Act
            var response = await client.PostAsJsonAsync("/api/Users", newUser);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdUser = await response.Content.ReadFromJsonAsync<User>();
            Assert.NotNull(createdUser);
            Assert.Equal("newuser", createdUser.Username);
        }

        // Test 3: PUT endpoint - Kullanıcıyı güncelle
        [Fact]
        public async Task PutUser_UpdatesExistingUser_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();
            
            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 200, Username = "oldname", Email = "old@test.com" });
            });

            var updatedUser = new User { Id = 200, Username = "newname", Email = "new@test.com" };

            // Act
            var response = await client.PutAsJsonAsync($"/api/Users/200", updatedUser);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        // Test 4: DELETE endpoint - Kullanıcıyı sil
        [Fact]
        public async Task DeleteUser_RemovesUser_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();
            
            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 300, Username = "deleteuser", Email = "delete@test.com" });
            });

            // Act
            var response = await client.DeleteAsync($"/api/Users/300");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        // Test 5: 404 Error - Olmayan Entry
        [Fact]
        public async Task GetEntry_ReturnsNotFound_WhenDoesNotExist_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();

            // Act
            var response = await client.GetAsync("/api/Entries/99999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // Test 6: 400 Error - Geçersiz UserId ile Entry oluşturma
        [Fact]
        public async Task PostEntry_ReturnsBadRequest_WhenInvalidUserId_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();
            var newEntry = new Entry { Id = 400, Title = "Test Entry", Content = "Test", UserId = 99999 };

            // Act
            var response = await client.PostAsJsonAsync("/api/Entries", newEntry);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // Test 7: User-Entry İlişkisi - Kullanıcı ile Entry oluşturma
        [Fact]
        public async Task User_Entry_Relationship_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 500, Username = "entryuser", Email = "entry@test.com" });
            });

            // Entry oluştur
            var entry = new Entry { Id = 500, Title = "User Entry", Content = "Content", UserId = 500 };

            // Act
            var response = await client.PostAsJsonAsync("/api/Entries", entry);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdEntry = await response.Content.ReadFromJsonAsync<Entry>();
            Assert.NotNull(createdEntry);
            Assert.Equal(500, createdEntry.UserId);
        }

        // Test 8: Entry-Review İlişkisi - Entry için Review oluşturma
        [Fact]
        public async Task Entry_Review_Relationship_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 600, Username = "reviewuser", Email = "review@test.com" });
                context.Entries.Add(new Entry { Id = 600, Title = "Review Entry", Content = "Content", UserId = 600 });
            });

            // Review oluştur
            var review = new Review { Id = 600, Text = "Great entry!", Rating = 5, EntryId = 600 };

            // Act
            var response = await client.PostAsJsonAsync("/api/Reviews", review);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdReview = await response.Content.ReadFromJsonAsync<Review>();
            Assert.NotNull(createdReview);
            Assert.Equal(600, createdReview.EntryId);
        }

        // Test 9: Entry-Tag İlişkisi - Entry için Tag oluşturma
        [Fact]
        public async Task Entry_Tag_Relationship_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 700, Username = "taguser", Email = "tag@test.com" });
                context.Entries.Add(new Entry { Id = 700, Title = "Tag Entry", Content = "Content", UserId = 700 });
            });

            // Tag oluştur
            var tag = new Tag { Id = 700, Name = "CSharp", EntryId = 700 };

            // Act
            var response = await client.PostAsJsonAsync("/api/Tags", tag);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdTag = await response.Content.ReadFromJsonAsync<Tag>();
            Assert.NotNull(createdTag);
            Assert.Equal(700, createdTag.EntryId);
        }

        // Test 10: Kompleks CRUD - User, Entry, Review ve Tag ile tam CRUD işlemleri
        [Fact]
        public async Task ComplexCRUD_UserWithEntriesAndReviews_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 800, Username = "complexuser", Email = "complex@test.com" });
            });
            
            // 2. Entry oluştur (CREATE)
            var entry = new Entry { Id = 800, Title = "Complex Entry", Content = "Content", UserId = 800 };
            var entryResponse = await client.PostAsJsonAsync("/api/Entries", entry);
            Assert.Equal(HttpStatusCode.Created, entryResponse.StatusCode);

            // 3. Review oluştur (CREATE)
            var review = new Review { Id = 800, Text = "Excellent!", Rating = 5, EntryId = 800 };
            var reviewResponse = await client.PostAsJsonAsync("/api/Reviews", review);
            Assert.Equal(HttpStatusCode.Created, reviewResponse.StatusCode);

            // 4. Entry'yi güncelle (UPDATE)
            entry.Title = "Updated Complex Entry";
            var updateResponse = await client.PutAsJsonAsync($"/api/Entries/{entry.Id}", entry);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            // 5. Review'i getir (READ)
            var getReviewResponse = await client.GetAsync($"/api/Reviews/{review.Id}");
            Assert.Equal(HttpStatusCode.OK, getReviewResponse.StatusCode);

            // 6. Review'i sil (DELETE)
            var deleteResponse = await client.DeleteAsync($"/api/Reviews/{review.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }

        // Test 11: Veritabanı CRUD işlemleri - Multiple Entries
        [Fact]
        public async Task DatabaseCRUD_MultipleEntries_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 900, Username = "multipleuser", Email = "multiple@test.com" });
            });

            // Birden fazla Entry oluştur
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

            // Act - Tüm Entry'leri getir
            var response = await client.GetAsync("/api/Entries");

            // Assert
            response.EnsureSuccessStatusCode();
            var entries = await response.Content.ReadFromJsonAsync<List<Entry>>();
            Assert.NotNull(entries);
            Assert.True(entries.Count >= 3);
        }

        // Test 12: Tag CRUD ve ilişki testi
        [Fact]
        public async Task Tag_CRUD_Operations_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();

            SeedDatabase(context =>
            {
                context.Users.Add(new User { Id = 1000, Username = "tagcruduser", Email = "tagcrud@test.com" });
                context.Entries.Add(new Entry { Id = 1000, Title = "Tag CRUD Entry", Content = "Content", UserId = 1000 });
            });

            // Tag oluştur (CREATE)
            var tag = new Tag { Id = 1000, Name = "Integration", EntryId = 1000 };
            var createResponse = await client.PostAsJsonAsync("/api/Tags", tag);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            // Tag getir (READ)
            var getResponse = await client.GetAsync($"/api/Tags/{tag.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            // Tag güncelle (UPDATE)
            tag.Name = "UpdatedTag";
            var updateResponse = await client.PutAsJsonAsync($"/api/Tags/{tag.Id}", tag);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            // Tag sil (DELETE)
            var deleteResponse = await client.DeleteAsync($"/api/Tags/{tag.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }

        // Test 13: Review ile 400 BadRequest testi
        [Fact]
        public async Task PostReview_ReturnsBadRequest_WhenInvalidEntryId_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();
            var review = new Review { Id = 1100, Text = "Invalid review", Rating = 3, EntryId = 99999 };

            // Act
            var response = await client.PostAsJsonAsync("/api/Reviews", review);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // Test 14: Tag ile 400 BadRequest testi
        [Fact]
        public async Task PostTag_ReturnsBadRequest_WhenInvalidEntryId_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();
            var tag = new Tag { Id = 1200, Name = "InvalidTag", EntryId = 99999 };

            // Act
            var response = await client.PostAsJsonAsync("/api/Tags", tag);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // Test 15: User bulunamadığında 404 testi
        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenDoesNotExist_IntegrationTest()
        {
            // Arrange
            var client = CreateClient();

            // Act
            var response = await client.GetAsync("/api/Users/88888");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
