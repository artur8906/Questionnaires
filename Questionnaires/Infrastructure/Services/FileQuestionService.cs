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
            Question question;
            switch (dto.Type)
            {
                case QuestionType.FiveStar:
                    int min = dto.MinValue ?? 1;
                    int max = dto.MaxValue ?? 5;

                    if (min < 1 || max > 10 || min >= max)
                        throw new ArgumentException("5-Star range must be between 1 and 10, and MinValue < MaxValue");

                    question = new FiveStarQuestion
                    {
                        Id = id,
                        Title = dto.Title,
                        MinValue = min,
                        MaxValue = max
                    };
                    break;

                case QuestionType.MultiSelect:
                    if (dto.Options == null || dto.Options.Count < 2)
                        throw new ArgumentException("Multi-select question requires at least 2 options.");

                    question = new MultiSelectQuestion
                    {
                        Id = id,
                        Title = dto.Title,
                        Options = dto.Options
                    };
                    break;

                case QuestionType.SingleSelect:
                    if (dto.Options == null || dto.Options.Count < 2)
                        throw new ArgumentException("Single-select question requires at least 2 options.");

                    question = new SingleSelectQuestion
                    {
                        Id = id,
                        Title = dto.Title,
                        Options = dto.Options
                    };
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dto.Type), "Unsupported question type.");
            }
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
            var question = await GetQuestionByIdAsync(questionId) ?? throw new ArgumentException("Question not found.");

            var id = Guid.NewGuid();

            QuestionResponse response = question.Type switch
            {
                QuestionType.FiveStar => dto.RatingValue is null
                    ? throw new ArgumentException("Rating is required.")
                    : new FiveStarQuestionRespons(id, questionId, dto.RespondentId, dto.RatingValue.Value),

                QuestionType.MultiSelect => dto.SelectedOptions is null || dto.SelectedOptions.Count < 2
                    ? throw new ArgumentException("At least 2 options are required.")
                    : new MultiSelectQuestionResponse(id, questionId, dto.RespondentId, dto.SelectedOptions),

                QuestionType.SingleSelect => string.IsNullOrWhiteSpace(dto.SelectedOption)
                    ? throw new ArgumentException("Option is required.")
                    : new SingleSelectQuestionResponse(id, questionId, dto.RespondentId, dto.SelectedOption),

                _ => throw new InvalidOperationException("Unsupported question type.")
            };

            await SaveResponseToFileAsync(response);
}

        private async Task SaveResponseToFileAsync(QuestionResponse response)
        {

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };

            List<QuestionResponse> responses;
            if (File.Exists(_responsesFilePath))
            {
                var existingData = await File.ReadAllTextAsync(_responsesFilePath);
                responses = JsonConvert.DeserializeObject<List<QuestionResponse>>(existingData, settings) ?? new List<QuestionResponse>();
            }
            else
            {
                responses = new List<QuestionResponse>();
            }
            responses.Add(response);
            var jsonData = JsonConvert.SerializeObject(responses, settings);
            await File.WriteAllTextAsync(_responsesFilePath, jsonData);
        }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            if (!File.Exists(_questionsFilePath))
                return new List<Question>();

            var json = await File.ReadAllTextAsync(_questionsFilePath);
            return JsonConvert.DeserializeObject<List<Question>>(json, settings) ?? new List<Question>();
        }

        public async Task<Question?> GetQuestionByIdAsync(Guid id)
        {
            var all = await GetAllQuestionsAsync();
            return all.FirstOrDefault(q => q.Id == id);
        }
    }
}
