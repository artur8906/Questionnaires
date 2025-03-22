namespace WepApi.DTOs
{
    public class SubmitResponseDto
    {
        public string RespondentId { get; set; } = null!;
        public int? RatingValue { get; set; }          
        public List<string>? SelectedOptions { get; set; } 
        public string? SelectedOption { get; set; }  
    }
}
