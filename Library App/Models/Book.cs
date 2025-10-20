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

        // ISBN support
        [Required, StringLength(20)]
        [RegularExpression(@"^[0-9Xx-]{10,20}$", ErrorMessage = "ISBN must be 10-20 chars, digits/X, dashes allowed")]
        public string Isbn { get; set; } = string.Empty;

        // Foreign key to Author
        [Required]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        // Optional Publisher
        public int? PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
    }
}
