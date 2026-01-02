using Software_Test_App.Models;
using Xunit;

namespace Software_Test_App.Tests
{
    public class ModelTests
    {
        [Fact]
        public void User_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@example.com"
            };

            // Act & Assert
            Assert.Equal(1, user.Id);
            Assert.Equal("testuser", user.Username);
            Assert.Equal("test@example.com", user.Email);
        }

        [Fact]
        public void Review_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var review = new Review
            {
                Id = 1,
                Text = "Great entry!",
                Rating = 5,
                EntryId = 10
            };

            // Act & Assert
            Assert.Equal(1, review.Id);
            Assert.Equal("Great entry!", review.Text);
            Assert.Equal(5, review.Rating);
            Assert.Equal(10, review.EntryId);
        }

        [Fact]
        public void Tag_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var tag = new Tag
            {
                Id = 1,
                Name = "CSharp",
                EntryId = 20
            };

            // Act & Assert
            Assert.Equal(1, tag.Id);
            Assert.Equal("CSharp", tag.Name);
            Assert.Equal(20, tag.EntryId);
        }
    }
}
