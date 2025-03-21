using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FiveStarQuestion : Question
    {
        public int MinValue { get; set; } = 1;
        public int MaxValue { get; set; } = 5; // Default if not specified

        public FiveStarQuestion()
        {
            Type = QuestionType.FiveStar;
        }
    }
}
