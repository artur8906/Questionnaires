using Core.DTOs;
using Core.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Controllers
{
    public class QuestionsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public QuestionsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task RootEndpoint_ShouldReturn404()
        {
            var response = await _client.GetAsync("/");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); // ok si no tienes "/"
        }
        [Fact]
        public async Task CreateAndGetQuestion_ShouldSucceed()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var dto = new CreateQuestionDto
            {
                Title = "FiveStar Test",
                Type = QuestionType.FiveStar,
                MinValue = 1,
                MaxValue = 5
            };

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var putResponse = await _client.PutAsync($"/questionnaires/{questionId}", content);
            var getResponse = await _client.GetAsync($"/questionnaires/{questionId}");

            // Assert
            putResponse.EnsureSuccessStatusCode();
            getResponse.EnsureSuccessStatusCode();

            var result = await getResponse.Content.ReadAsStringAsync();
            result.Should().Contain("FiveStar Test");
        }
    }

}
