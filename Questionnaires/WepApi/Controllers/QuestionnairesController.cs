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
            return Ok(); 
        }

        [HttpPost("{id:guid}/responses")]
        public async Task<IActionResult> SubmitResponse(Guid id, [FromBody] SubmitResponseDto dto)
        {
            await _questionService.SubmitResponseAsync(id, dto);
            return Ok();
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);

            if (question == null)
                return NotFound();

            return Ok(question);
        }
    } 
    }
