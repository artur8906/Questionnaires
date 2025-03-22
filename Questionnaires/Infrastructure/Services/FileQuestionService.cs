using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class FileQuestionService: IQuestionService
    {
        private readonly string _questionsFilePath;
        private readonly string _responsesFilePath;
    
        public FileQuestionService(string questionsFilePath, string responsesFilePath)
        {
            _questionsFilePath = questionsFilePath;
            _responsesFilePath = responsesFilePath;
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
             var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };

            List<Question> questions;
            if (File.Exists(_questionsFilePath))
            {
                var existingData = await File.ReadAllTextAsync(_questionsFilePath);
                questions = JsonConvert.DeserializeObject<List<Question>>(existingData, settings)?? new List<Question>();
            }
            else
            {
                questions = new List<Question>();
            }
            questions.Add(question);
            var jsonData = JsonConvert.SerializeObject(questions, settings);
            await File.WriteAllTextAsync(_questionsFilePath, jsonData);
        }

        public async Task SubmitResponseAsync(Guid questionId, SubmitResponseDto dto)
        {
            var response = new QuestionResponse
            {
                Id = Guid.NewGuid(),
                QuestionId = questionId,
                RespondentId = dto.RespondentId
            };

            await SaveResponseToFileAsync(response);
        }

        private async Task SaveResponseToFileAsync(QuestionResponse response)
        {
            List<QuestionResponse> responses;

            if (File.Exists(_responsesFilePath))
            {
                var existingData = await File.ReadAllTextAsync(_responsesFilePath);
            
            }
            else
            {
                responses = new List<QuestionResponse>();
            }



            var jsonData = "null";

            await File.WriteAllTextAsync(_responsesFilePath, jsonData);
        }
    }

}
