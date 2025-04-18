﻿using Core.DTOs;
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
             await SaveItemToFileAsync(question, _questionsFilePath);  
        }

        public async Task SubmitResponseAsync(Guid questionId, SubmitResponseDto dto)
        {
            var question = await GetQuestionByIdAsync(questionId) ?? throw new ArgumentException("Question not found.");

            var id = Guid.NewGuid();

            QuestionResponse response;

            switch (question)
            {
                case FiveStarQuestion fiveStar:
                    if (dto.RatingValue is null)
                        throw new ArgumentException("Rating is required.");
                    if (dto.RatingValue < fiveStar.MinValue || dto.RatingValue > fiveStar.MaxValue)
                        throw new ArgumentException($"Rating must be between {fiveStar.MinValue} and {fiveStar.MaxValue}.");
                    response = new FiveStarQuestionRespons(id, questionId, dto.RespondentId, dto.RatingValue.Value);
                    break;

                case MultiSelectQuestion multiSelect:
                    if (dto.SelectedOptions is null || dto.SelectedOptions.Count < 2)
                        throw new ArgumentException("At least 2 options are required.");
                    if (!dto.SelectedOptions.All(opt => multiSelect.Options.Contains(opt)))
                        throw new ArgumentException("One or more selected options are not valid.");
                    response = new MultiSelectQuestionResponse(id, questionId, dto.RespondentId, dto.SelectedOptions);
                    break;

                case SingleSelectQuestion singleSelect:
                    if (string.IsNullOrWhiteSpace(dto.SelectedOption))
                        throw new ArgumentException("Selected option is required.");
                    if (!singleSelect.Options.Contains(dto.SelectedOption))
                        throw new ArgumentException("Selected option is not valid.");
                    response = new SingleSelectQuestionResponse(id, questionId, dto.RespondentId, dto.SelectedOption);
                    break;

                default:
                    throw new InvalidOperationException("Unsupported question type.");
            }

            await SaveItemToFileAsync(response, _responsesFilePath);
        }

        private async Task SaveItemToFileAsync<T>(T item, string filePath) where T : IHasId
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };

            List<T> items;
            // Read existing data, if any
            if (File.Exists(filePath))
            {
                var existingData = await File.ReadAllTextAsync(filePath);
                items = JsonConvert.DeserializeObject<List<T>>(existingData, settings)?? new List<T>();
            }
            else
            {
                items = new List<T>();
            }

            bool alreadyExists = items.Any(existingItem => existingItem.Id == item.Id);
            if (alreadyExists)
            {
                throw new InvalidOperationException($"Item with ID {item.Id} already exists in {filePath}.");
            }
            // Add the new item
            items.Add(item);

            // Serialize and write to file
            var jsonData = JsonConvert.SerializeObject(items, settings);
            await File.WriteAllTextAsync(filePath, jsonData);
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
