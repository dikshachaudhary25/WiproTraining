namespace MyApiApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? PublishDate { get; set; } = DateTime.Now;
    }
}