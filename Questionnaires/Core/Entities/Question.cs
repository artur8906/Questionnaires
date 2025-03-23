using Core.Enums;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public abstract class Question: IHasId
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public QuestionType Type { get; protected set; }
    }
}
