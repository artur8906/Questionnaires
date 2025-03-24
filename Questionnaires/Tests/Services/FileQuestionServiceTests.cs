using Core.DTOs;
using Core.Enums;
using Infrastructure.Services;

namespace Tests
{
    public class FileQuestionServiceTests
    {
        private string GetTempFile() => Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        [Fact]
        public async Task CreateQuestionAsync_Creates_Valid_FiveStarQuestion()
        {
            // Arrange
            var questionsFile = GetTempFile();
            var responsesFile = GetTempFile();
            var service = new FileQuestionService(questionsFile, responsesFile);
            var dto = new CreateQuestionDto
            {
                Title = "Rate this test",
                Type = QuestionType.FiveStar,
                MinValue = 2,
                MaxValue = 8
            };
            var id = Guid.NewGuid();
            // Act
            await service.CreateQuestionAsync(id, dto);
            // Assert
            var fileContent = await File.ReadAllTextAsync(questionsFile);
            Assert.Contains("Rate this test", fileContent);
            Assert.Contains("FiveStar", fileContent);
        }

        [Fact]
        public async Task CreateQuestionAsync_Throws_For_Invalid_Range()
        {
            // Arrange
            var service = new FileQuestionService(GetTempFile(), GetTempFile());
            var dto = new CreateQuestionDto
            {
                Title = "Invalid range",
                Type = QuestionType.FiveStar,
                MinValue = 0, 
                MaxValue = 11 
            };
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateQuestionAsync(Guid.NewGuid(), dto));
        }
        
    }
}