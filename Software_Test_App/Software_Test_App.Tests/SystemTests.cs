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
    public class SystemTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public SystemTests(CustomWebApplicationFactory factory)
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
        public async Task CompleteUserJourney_CreateUser_AddEntry_AddReview_AddTag_SystemTest()
        {
            var client = CreateClient();

            var newUser = new User 
            { 
                Id = 10001, 
                Username = "johndoe", 
                Email = "johndoe@example.com" 
            };
            
            var userResponse = await client.PostAsJsonAsync("/api/Users", newUser);
            Assert.Equal(HttpStatusCode.Created, userResponse.StatusCode);
            var createdUser = await userResponse.Content.ReadFromJsonAsync<User>();
            Assert.NotNull(createdUser);

            var newEntry = new Entry
            {
                Id = 10001,
                Title = "Yazılım Test Teknikleri Hakkında Düşüncelerim",
                Content = "Yazılım testleri, kaliteli yazılım geliştirmenin temel taşlarından biridir...",
                UserId = createdUser.Id
            };

            var entryResponse = await client.PostAsJsonAsync("/api/Entries", newEntry);
            Assert.Equal(HttpStatusCode.Created, entryResponse.StatusCode);
            var createdEntry = await entryResponse.Content.ReadFromJsonAsync<Entry>();
            Assert.NotNull(createdEntry);
            Assert.Equal(newEntry.Title, createdEntry.Title);

            var newReview = new Review
            {
                Id = 10001,
                Text = "Çok faydalı bir yazı olmuş, eline sağlık!",
                Rating = 5,
                EntryId = createdEntry.Id
            };

            var reviewResponse = await client.PostAsJsonAsync("/api/Reviews", newReview);
            Assert.Equal(HttpStatusCode.Created, reviewResponse.StatusCode);
            var createdReview = await reviewResponse.Content.ReadFromJsonAsync<Review>();
            Assert.NotNull(createdReview);
            Assert.Equal(5, createdReview.Rating);

            var tag1 = new Tag { Id = 10001, Name = "SoftwareTesting", EntryId = createdEntry.Id };
            var tag2 = new Tag { Id = 10002, Name = "QualityAssurance", EntryId = createdEntry.Id };

            var tag1Response = await client.PostAsJsonAsync("/api/Tags", tag1);
            var tag2Response = await client.PostAsJsonAsync("/api/Tags", tag2);

            Assert.Equal(HttpStatusCode.Created, tag1Response.StatusCode);
            Assert.Equal(HttpStatusCode.Created, tag2Response.StatusCode);

            var finalEntryResponse = await client.GetAsync($"/api/Entries/{createdEntry.Id}");
            Assert.Equal(HttpStatusCode.OK, finalEntryResponse.StatusCode);
            var finalEntry = await finalEntryResponse.Content.ReadFromJsonAsync<Entry>();
            Assert.NotNull(finalEntry);
            Assert.Equal("Yazılım Test Teknikleri Hakkında Düşüncelerim", finalEntry.Title);
        }

        [Fact]
        public async Task ContentManagement_List_View_Update_Delete_SystemTest()
        {
            var client = CreateClient();

            SeedDatabase(context =>
            {
                var user = new User { Id = 10100, Username = "contentmanager", Email = "manager@example.com" };
                context.Users.Add(user);
                context.Entries.Add(new Entry 
                { 
                    Id = 10100, 
                    Title = "Eski Başlık", 
                    Content = "Eski içerik metni",
                    UserId = user.Id 
                });
            });

            var listResponse = await client.GetAsync("/api/Entries");
            Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);
            var entries = await listResponse.Content.ReadFromJsonAsync<List<Entry>>();
            Assert.NotNull(entries);
            Assert.True(entries.Count > 0);

            var detailResponse = await client.GetAsync("/api/Entries/10100");
            Assert.Equal(HttpStatusCode.OK, detailResponse.StatusCode);
            var entry = await detailResponse.Content.ReadFromJsonAsync<Entry>();
            Assert.NotNull(entry);
            Assert.Equal("Eski Başlık", entry.Title);

            entry.Title = "Güncellenmiş Başlık";
            entry.Content = "Bu içerik kullanıcı tarafından düzenlendi ve yeni bilgiler eklendi.";
            
            var updateResponse = await client.PutAsJsonAsync($"/api/Entries/{entry.Id}", entry);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var verifyResponse = await client.GetAsync($"/api/Entries/{entry.Id}");
            var updatedEntry = await verifyResponse.Content.ReadFromJsonAsync<Entry>();
            Assert.Equal("Güncellenmiş Başlık", updatedEntry?.Title);

            var deleteResponse = await client.DeleteAsync($"/api/Entries/{entry.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var verifyDeleteResponse = await client.GetAsync($"/api/Entries/{entry.Id}");
            Assert.Equal(HttpStatusCode.NotFound, verifyDeleteResponse.StatusCode);
        }

        [Fact]
        public async Task SearchWorkflow_CreateEntries_PerformSearch_ViewResults_SystemTest()
        {
            var client = CreateClient();

            SeedDatabase(context =>
            {
                var user = new User { Id = 10200, Username = "researcher", Email = "research@example.com" };
                context.Users.Add(user);
                
                context.Entries.Add(new Entry 
                { 
                    Id = 10201, 
                    Title = "C# Programlama Dili", 
                    Content = "C# modern bir programlama dilidir",
                    UserId = user.Id 
                });
                context.Entries.Add(new Entry 
                { 
                    Id = 10202, 
                    Title = "Python ile Veri Analizi", 
                    Content = "Python veri bilimi için harika bir dildir",
                    UserId = user.Id 
                });
                context.Entries.Add(new Entry 
                { 
                    Id = 10203, 
                    Title = "JavaScript Framework'leri", 
                    Content = "React, Vue ve Angular popüler JavaScript framework'leridir",
                    UserId = user.Id 
                });
            });

            var searchTerm = "Python";
            var searchResponse = await client.GetAsync($"/api/Search/query?q={searchTerm}");
            Assert.Equal(HttpStatusCode.OK, searchResponse.StatusCode);

            var searchResults = await searchResponse.Content.ReadFromJsonAsync<List<Entry>>();
            Assert.NotNull(searchResults);
            Assert.NotEmpty(searchResults);
            Assert.Contains(searchResults, e => e.Title.Contains("Python") || e.Content.Contains("Python"));

            var searchLog = new SearchLog 
            { 
                Id = 10201, 
                Query = searchTerm, 
                Timestamp = DateTime.UtcNow 
            };
            var saveSearchResponse = await client.PostAsJsonAsync("/api/Search", searchLog);
            Assert.Equal(HttpStatusCode.Created, saveSearchResponse.StatusCode);

            var historyResponse = await client.GetAsync("/api/Search");
            Assert.Equal(HttpStatusCode.OK, historyResponse.StatusCode);
            var history = await historyResponse.Content.ReadFromJsonAsync<List<SearchLog>>();
            Assert.NotNull(history);
            Assert.Contains(history, h => h.Query == searchTerm);
        }

        [Fact]
        public async Task MultiUserInteraction_MultipleUsers_CrossReviews_SystemTest()
        {
            var client = CreateClient();
            var user1 = new User { Id = 10301, Username = "alice", Email = "alice@example.com" };
            var user1Response = await client.PostAsJsonAsync("/api/Users", user1);
            Assert.Equal(HttpStatusCode.Created, user1Response.StatusCode);

            var entry1 = new Entry
            {
                Id = 10301,
                Title = "Test Otomasyonunun Önemi",
                Content = "Test otomasyonu zaman kazandırır ve hataları azaltır",
                UserId = user1.Id
            };
            var entry1Response = await client.PostAsJsonAsync("/api/Entries", entry1);
            Assert.Equal(HttpStatusCode.Created, entry1Response.StatusCode);

            // 2. Adım: İkinci kullanıcıyı oluştur ve entry ekle
            var user2 = new User { Id = 10302, Username = "bob", Email = "bob@example.com" };
            var user2Response = await client.PostAsJsonAsync("/api/Users", user2);
            Assert.Equal(HttpStatusCode.Created, user2Response.StatusCode);

            var entry2 = new Entry
            {
                Id = 10302,
                Title = "Agile Metodolojiler",
                Content = "Scrum ve Kanban çevik geliştirme metodolojileridir",
                UserId = user2.Id
            };
            var entry2Response = await client.PostAsJsonAsync("/api/Entries", entry2);
            Assert.Equal(HttpStatusCode.Created, entry2Response.StatusCode);

            // 3. Adım: Üçüncü kullanıcı oluştur (reviewer olarak)
            var user3 = new User { Id = 10303, Username = "charlie", Email = "charlie@example.com" };
            var user3Response = await client.PostAsJsonAsync("/api/Users", user3);
            Assert.Equal(HttpStatusCode.Created, user3Response.StatusCode);

            // 4. Adım: Farklı kullanıcıların birbirlerinin entry'lerine review yapması
            var review1 = new Review
            {
                Id = 10301,
                Text = "Harika bir bakış açısı, çok yararlı bilgiler",
                Rating = 5,
                EntryId = entry1.Id
            };
            var review1Response = await client.PostAsJsonAsync("/api/Reviews", review1);
            Assert.Equal(HttpStatusCode.Created, review1Response.StatusCode);

            var review2 = new Review
            {
                Id = 10302,
                Text = "İyi bir özet ama daha detaylı olabilirdi",
                Rating = 4,
                EntryId = entry2.Id
            };
            var review2Response = await client.PostAsJsonAsync("/api/Reviews", review2);
            Assert.Equal(HttpStatusCode.Created, review2Response.StatusCode);

            var review3 = new Review
            {
                Id = 10303,
                Text = "Agile konusunda çok net anlatım",
                Rating = 5,
                EntryId = entry2.Id
            };
            var review3Response = await client.PostAsJsonAsync("/api/Reviews", review3);
            Assert.Equal(HttpStatusCode.Created, review3Response.StatusCode);

            // 5. Adım: Tüm review'ları getir ve doğrula
            var allReviewsResponse = await client.GetAsync("/api/Reviews");
            Assert.Equal(HttpStatusCode.OK, allReviewsResponse.StatusCode);
            var allReviews = await allReviewsResponse.Content.ReadFromJsonAsync<List<Review>>();
            Assert.NotNull(allReviews);
            Assert.True(allReviews.Count >= 3);
        }

        [Fact]
        public async Task ComplexBusinessScenario_FullWorkflow_WithModifications_SystemTest()
        {
            var client = CreateClient();

            var mainUser = new User { Id = 10401, Username = "author", Email = "author@example.com" };
            await client.PostAsJsonAsync("/api/Users", mainUser);

            var article = new Entry
            {
                Id = 10401,
                Title = "Yazılım Kalite Güvencesi: Kapsamlı Bir Bakış",
                Content = "Bu makalede yazılım kalite güvencesinin tüm yönlerini ele alacağız...",
                UserId = mainUser.Id
            };
            var articleResponse = await client.PostAsJsonAsync("/api/Entries", article);
            Assert.Equal(HttpStatusCode.Created, articleResponse.StatusCode);
            var reviews = new List<Review>
            {
                new Review { Id = 10401, Text = "Mükemmel bir kaynak", Rating = 5, EntryId = article.Id },
                new Review { Id = 10402, Text = "Çok bilgilendirici", Rating = 5, EntryId = article.Id },
                new Review { Id = 10403, Text = "İyi ama bazı kısımlar eksik", Rating = 3, EntryId = article.Id },
                new Review { Id = 10404, Text = "Ortalama bir yazı", Rating = 3, EntryId = article.Id }
            };

            foreach (var review in reviews)
            {
                var reviewResponse = await client.PostAsJsonAsync("/api/Reviews", review);
                Assert.Equal(HttpStatusCode.Created, reviewResponse.StatusCode);
            }

            var tags = new List<Tag>
            {
                new Tag { Id = 10401, Name = "QualityAssurance", EntryId = article.Id },
                new Tag { Id = 10402, Name = "Testing", EntryId = article.Id },
                new Tag { Id = 10403, Name = "BestPractices", EntryId = article.Id }
            };

            foreach (var tag in tags)
            {
                var tagResponse = await client.PostAsJsonAsync("/api/Tags", tag);
                Assert.Equal(HttpStatusCode.Created, tagResponse.StatusCode);
            }

            article.Content = "Bu makalede yazılım kalite güvencesinin tüm yönlerini detaylı şekilde ele alacağız. Güncellenmiş içerik...";
            var updateResponse = await client.PutAsJsonAsync($"/api/Entries/{article.Id}", article);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var deleteReviewResponse = await client.DeleteAsync($"/api/Reviews/10404");
            Assert.Equal(HttpStatusCode.NoContent, deleteReviewResponse.StatusCode);

            var finalCheckResponse = await client.GetAsync($"/api/Entries/{article.Id}");
            Assert.Equal(HttpStatusCode.OK, finalCheckResponse.StatusCode);
            var finalArticle = await finalCheckResponse.Content.ReadFromJsonAsync<Entry>();
            Assert.NotNull(finalArticle);
            Assert.Contains("Güncellenmiş", finalArticle.Content);

            var remainingReviewsResponse = await client.GetAsync("/api/Reviews");
            var remainingReviews = await remainingReviewsResponse.Content.ReadFromJsonAsync<List<Review>>();
            Assert.NotNull(remainingReviews);
            Assert.DoesNotContain(remainingReviews, r => r.Id == 10404);
        }

        [Fact]
        public async Task ErrorHandling_InvalidOperations_ProperErrorCodes_SystemTest()
        {
            var client = CreateClient();

            var invalidReview = new Review
            {
                Id = 10501,
                Text = "Bu review geçersiz bir entry için",
                Rating = 5,
                EntryId = 99999
            };
            var invalidReviewResponse = await client.PostAsJsonAsync("/api/Reviews", invalidReview);
            Assert.Equal(HttpStatusCode.BadRequest, invalidReviewResponse.StatusCode);

            var invalidEntry = new Entry
            {
                Id = 10501,
                Title = "Geçersiz Entry",
                Content = "Bu entry geçersiz bir kullanıcı için",
                UserId = 99999
            };
            var invalidEntryResponse = await client.PostAsJsonAsync("/api/Entries", invalidEntry);
            Assert.Equal(HttpStatusCode.BadRequest, invalidEntryResponse.StatusCode);

            var notFoundResponse = await client.GetAsync("/api/Entries/99999");
            Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);

            var invalidTag = new Tag { Id = 99999, Name = "InvalidTag", EntryId = 99999 };
            var updateInvalidResponse = await client.PutAsJsonAsync("/api/Tags/99999", invalidTag);
            Assert.Equal(HttpStatusCode.NotFound, updateInvalidResponse.StatusCode);

            var deleteInvalidResponse = await client.DeleteAsync("/api/Reviews/99999");
            Assert.Equal(HttpStatusCode.NotFound, deleteInvalidResponse.StatusCode);
        }

        [Fact]
        public async Task BulkDataProcessing_CreateMultiple_ListAndFilter_SystemTest()
        {
            var client = CreateClient();
            var users = new List<User>();
            for (int i = 1; i <= 3; i++)
            {
                var user = new User 
                { 
                    Id = 10600 + i, 
                    Username = $"bulkuser{i}", 
                    Email = $"bulk{i}@example.com" 
                };
                var userResponse = await client.PostAsJsonAsync("/api/Users", user);
                Assert.Equal(HttpStatusCode.Created, userResponse.StatusCode);
                users.Add(user);
            }

            var entryIdCounter = 10601;
            foreach (var user in users)
            {
                for (int j = 1; j <= 2; j++)
                {
                    var entry = new Entry
                    {
                        Id = entryIdCounter++,
                        Title = $"Entry {j} by {user.Username}",
                        Content = $"Bu içerik {user.Username} tarafından oluşturuldu",
                        UserId = user.Id
                    };
                    var entryResponse = await client.PostAsJsonAsync("/api/Entries", entry);
                    Assert.Equal(HttpStatusCode.Created, entryResponse.StatusCode);
                }
            }

            var allEntriesResponse = await client.GetAsync("/api/Entries");
            Assert.Equal(HttpStatusCode.OK, allEntriesResponse.StatusCode);
            var allEntries = await allEntriesResponse.Content.ReadFromJsonAsync<List<Entry>>();
            Assert.NotNull(allEntries);
            Assert.True(allEntries.Count >= 6);

            var tagIdCounter = 10601;
            foreach (var entry in allEntries.Where(e => e.Id >= 10601 && e.Id < 10607))
            {
                var tag = new Tag
                {
                    Id = tagIdCounter++,
                    Name = $"Tag{tagIdCounter}",
                    EntryId = entry.Id
                };
                var tagResponse = await client.PostAsJsonAsync("/api/Tags", tag);
                Assert.Equal(HttpStatusCode.Created, tagResponse.StatusCode);
            }

            var allTagsResponse = await client.GetAsync("/api/Tags");
            Assert.Equal(HttpStatusCode.OK, allTagsResponse.StatusCode);
            var allTags = await allTagsResponse.Content.ReadFromJsonAsync<List<Tag>>();
            Assert.NotNull(allTags);
            Assert.True(allTags.Count >= 6);
        }

        [Fact]
        public async Task DataIntegrity_RelatedResources_ConsistencyCheck_SystemTest()
        {
            var client = CreateClient();
            var baseUser = new User { Id = 10701, Username = "integrityuser", Email = "integrity@example.com" };
            await client.PostAsJsonAsync("/api/Users", baseUser);

            var baseEntry = new Entry
            {
                Id = 10701,
                Title = "Veri Bütünlüğü Testi",
                Content = "Bu entry veri bütünlüğü için oluşturuldu",
                UserId = baseUser.Id
            };
            await client.PostAsJsonAsync("/api/Entries", baseEntry);

            var review1 = new Review { Id = 10701, Text = "İlk review", Rating = 4, EntryId = baseEntry.Id };
            var review2 = new Review { Id = 10702, Text = "İkinci review", Rating = 5, EntryId = baseEntry.Id };
            await client.PostAsJsonAsync("/api/Reviews", review1);
            await client.PostAsJsonAsync("/api/Reviews", review2);

            var tag1 = new Tag { Id = 10701, Name = "Integrity", EntryId = baseEntry.Id };
            var tag2 = new Tag { Id = 10702, Name = "Testing", EntryId = baseEntry.Id };
            await client.PostAsJsonAsync("/api/Tags", tag1);
            await client.PostAsJsonAsync("/api/Tags", tag2);

            var entryResponse = await client.GetAsync($"/api/Entries/{baseEntry.Id}");
            Assert.Equal(HttpStatusCode.OK, entryResponse.StatusCode);
            var fetchedEntry = await entryResponse.Content.ReadFromJsonAsync<Entry>();
            Assert.NotNull(fetchedEntry);
            Assert.Equal(baseUser.Id, fetchedEntry.UserId);

            var reviewsResponse = await client.GetAsync("/api/Reviews");
            var reviews = await reviewsResponse.Content.ReadFromJsonAsync<List<Review>>();
            var relatedReviews = reviews?.Where(r => r.EntryId == baseEntry.Id).ToList();
            Assert.NotNull(relatedReviews);
            Assert.True(relatedReviews.Count >= 2);

            var tagsResponse = await client.GetAsync("/api/Tags");
            var tags = await tagsResponse.Content.ReadFromJsonAsync<List<Tag>>();
            var relatedTags = tags?.Where(t => t.EntryId == baseEntry.Id).ToList();
            Assert.NotNull(relatedTags);
            Assert.True(relatedTags.Count >= 2);

            fetchedEntry.Title = "Güncellenmiş Başlık";
            var updateResponse = await client.PutAsJsonAsync($"/api/Entries/{baseEntry.Id}", fetchedEntry);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var verifyResponse = await client.GetAsync($"/api/Entries/{baseEntry.Id}");
            var verifiedEntry = await verifyResponse.Content.ReadFromJsonAsync<Entry>();
            Assert.NotNull(verifiedEntry);
            Assert.Equal("Güncellenmiş Başlık", verifiedEntry.Title);
            Assert.Equal(baseUser.Id, verifiedEntry.UserId);
        }
    }
}

