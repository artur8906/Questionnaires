using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class MultiSelectQuestionResponse:QuestionResponse
    {
        public List<string>? SelectedOptions { get; set; }
        public MultiSelectQuestionResponse(Guid id, Guid questionId, Guid respondentId, List<string> options) : base(id, questionId, respondentId)
        {
            SelectedOptions = options;
        }
    }
}
