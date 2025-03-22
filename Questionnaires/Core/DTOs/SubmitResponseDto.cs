namespace Core.DTOs
{
    public class SubmitResponseDto
    {
        public Guid QuestionId { get; set; }
        public Guid RespondentId { get; set; }
        public int? RatingValue { get; set; }          
        public List<string>? SelectedOptions { get; set; } 
        public string? SelectedOption { get; set; }  
    }
}
