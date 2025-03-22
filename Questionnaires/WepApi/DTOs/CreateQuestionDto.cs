using Core.Enums;

namespace WepApi.DTOs
{
    public class CreateQuestionDto
    {
        public string Title { get; set; } = null!;
        public QuestionType Type { get; set; }

        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }

        public List<string>? Options { get; set; }
    }
}
