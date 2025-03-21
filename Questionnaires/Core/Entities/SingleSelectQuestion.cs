using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SingleSelectQuestion : Question
    {
        public List<string> Options { get; set; } = new();

        public SingleSelectQuestion()
        {
            Type = QuestionType.SingleSelect;
        }
    }
}
