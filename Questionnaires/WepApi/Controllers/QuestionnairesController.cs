using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace WepApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionnairesController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionnairesController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> CreateQuestion(Guid id, [FromBody] CreateQuestionDto dto)
        {
            await _questionService.CreateQuestionAsync(id, dto);
            return CreatedAtAction(nameof(GetQuestion), new { id }, null);
        }

        private object GetQuestion()
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id:guid}/responses")]
        public async Task<IActionResult> SubmitResponse(Guid id, [FromBody] SubmitResponseDto dto)
        {
            await _questionService.SubmitResponseAsync(id, dto);
            return Ok();
        }
           } 
    }
