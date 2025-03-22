using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SingleSelectQuestionResponse: QuestionResponse
    {
        public string? SelectedOption { get; set; }
        public SingleSelectQuestionResponse(Guid id, Guid questionId, Guid respondentId, string option): base(id, questionId, respondentId)
        {
            SelectedOption = option;
        }

    }
}
