using Library_App.Data;
using Library_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_App.Services;

public class BookService(Library_AppContext db) : IBookService
{
    public Task<List<Book>> GetAllAsync() => db.Books.Include(b=>b.Author).ToListAsync();
    public Task<Book?> GetAsync(int id) => db.Books.Include(b=>b.Author).FirstOrDefaultAsync(b=>b.Id==id);
    public async Task<Book> CreateAsync(string title, int year, int authorId)
    {
        var book = new Book { Title = title, Year = year, AuthorId = authorId };
        db.Books.Add(book);
        await db.SaveChangesAsync();
        await db.Entry(book).Reference(b=>b.Author).LoadAsync();
        return book;
    }
}
