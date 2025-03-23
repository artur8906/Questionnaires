using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public abstract class QuestionResponse: IHasId
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public Guid RespondentId { get; set; }
        protected QuestionResponse(Guid id, Guid questionId, Guid respondentId)
        {
            Id = id;
            QuestionId = questionId;
            RespondentId = respondentId;
        }
    }
}
