namespace TechLibrary.Application.DTOs.Books.Response
{
    public class ResponseBookDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}
