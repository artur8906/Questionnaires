using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IQuestionService
    {
        Task CreateQuestionAsync(Guid id, CreateQuestionDto dto);
        Task<Question?> GetQuestionByIdAsync(Guid id);
        Task SubmitResponseAsync(Guid questionId, SubmitResponseDto dto);
    }
}
