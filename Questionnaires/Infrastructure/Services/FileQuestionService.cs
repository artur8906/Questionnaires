
using Core.Entities;
using Core.Enums;
using System.Text.Json;
using WepApi.DTOs;

namespace Infrastructure.Services
{
    public class FileQuestionService
    {
        private readonly string _filePath;

        public FileQuestionService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task CreateQuestionAsync(Guid id, CreateQuestionDto dto)
        {
            Question question = dto.Type switch
            {
                QuestionType.FiveStar => new FiveStarQuestion
                {
                    Id = id,
                    Title = dto.Title,
                    MinValue = dto.MinValue ?? 1,
                    MaxValue = dto.MaxValue ?? 5
                },
                QuestionType.MultiSelect => new MultiSelectQuestion
                {
                    Id = id,
                    Title = dto.Title,
                    Options = dto.Options?.Count >= 2
                        ? dto.Options
                        : throw new ArgumentException("At least 2 options required.")
                },
                QuestionType.SingleSelect => new SingleSelectQuestion
                {
                    Id = id,
                    Title = dto.Title,
                    Options = dto.Options?.Count >= 2
                        ? dto.Options
                        : throw new ArgumentException("At least 2 options required.")
                },
                _ => throw new ArgumentException("Unsupported question type.")
            };

            await SaveQuestionToFileAsync(question);
        }

        private async Task SaveQuestionToFileAsync(Question question)
        {
            List<Question> questions;

            if (File.Exists(_filePath))
            {
                var existingData = await File.ReadAllTextAsync(_filePath);
                questions = JsonSerializer.Deserialize<List<Question>>(existingData,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true })
                    ?? new List<Question>();
            }
            else
            {
                questions = new List<Question>();
            }

            questions.Add(question);

            var jsonData = JsonSerializer.Serialize(questions,
                new JsonSerializerOptions { WriteIndented = true });

            await File.WriteAllTextAsync(_filePath, jsonData);
        }
    }

}
