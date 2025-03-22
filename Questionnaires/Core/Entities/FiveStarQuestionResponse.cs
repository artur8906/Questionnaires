using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FiveStarQuestionRespons : QuestionResponse 
    {
        public int? RatingValue { get; set; }
        public FiveStarQuestionRespons(Guid id, Guid questionId, Guid respondentId, int ratingValue) : base(id, questionId, respondentId)
        {
            RatingValue = ratingValue;
        }
    }
}
