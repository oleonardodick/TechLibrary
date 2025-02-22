using TechLibrary.Domain.Common;

namespace TechLibrary.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int Amount { get; set; } 
    }
}
