using Software_Test_App.Models;
using Xunit;

namespace Software_Test_App.Tests
{
    public class EntryTests
    {
        [Fact]
        public void Entry_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var entry = new Entry
            {
                Id = 1,
                Title = "Test Entry",
                Content = "Test Content",
                UserId = 10
            };

            // Act & Assert
            Assert.Equal(1, entry.Id);
            Assert.Equal("Test Entry", entry.Title);
            Assert.Equal("Test Content", entry.Content);
            Assert.Equal(10, entry.UserId);
        }
    }
}
