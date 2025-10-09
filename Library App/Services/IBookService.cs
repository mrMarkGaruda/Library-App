using Library_App.Models;
namespace Library_App.Services;

public interface IBookService
{
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetAsync(int id);
    Task<Book> CreateAsync(string title, int year, int authorId);
}
