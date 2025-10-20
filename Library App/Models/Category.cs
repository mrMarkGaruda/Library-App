using System.ComponentModel.DataAnnotations;

namespace Library_App.Models;

public class Category
{
    public int Id { get; set; }
    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
