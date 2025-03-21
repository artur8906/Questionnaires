using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string RespondentId { get; set; } = null!;
        public string Response { get; set; } = null!;
    }
}
