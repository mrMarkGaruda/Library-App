namespace Library_App.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Book
    {
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;
        [Range(0, 3000)]
        public int Year { get; set; }

        // Foreign key to Author
        [Required]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
