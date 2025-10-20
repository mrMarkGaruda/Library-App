using Library_App.Data;
using Library_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_App.Services;

public class BookService(Library_AppContext db, IAuthorService authors, IPublisherService publishers, IGoogleBooksService google) : IBookService
{
    public Task<List<Book>> GetAllAsync() => db.Books.Include(b=>b.Author).Include(b=>b.Publisher).ToListAsync();
    public Task<Book?> GetAsync(int id) => db.Books.Include(b=>b.Author).Include(b=>b.Publisher).FirstOrDefaultAsync(b=>b.Id==id);
    public Task<Book?> GetByIsbnAsync(string isbn) => db.Books.Include(b=>b.Author).Include(b=>b.Publisher).FirstOrDefaultAsync(b=>b.Isbn==isbn);

    public async Task<Book> CreateAsync(string title, int year, int authorId, string isbn, int? publisherId = null)
    {
        var book = new Book { Title = title, Year = year, AuthorId = authorId, Isbn = isbn, PublisherId = publisherId };
        db.Books.Add(book);
        await db.SaveChangesAsync();
        await db.Entry(book).Reference(b=>b.Author).LoadAsync();
        await db.Entry(book).Reference(b=>b.Publisher!).LoadAsync();
        return book;
    }

    public async Task<Book> ImportByIsbnAsync(string isbn)
    {
        // If already exists, return it
        var existing = await GetByIsbnAsync(isbn);
        if (existing is not null) return existing;

        var info = await google.GetByIsbnAsync(isbn) ?? throw new InvalidOperationException("Book not found in Google Books API");
        // Ensure author exists (take first author if multiple)
        var authorName = info.Authors.FirstOrDefault() ?? "Unknown";
        var author = await authors.GetByNameOrCreateAsync(authorName);

        int? publisherId = null;
        if (!string.IsNullOrWhiteSpace(info.Publisher))
        {
            var publisher = await publishers.GetByNameOrCreateAsync(info.Publisher);
            publisherId = publisher.Id;
        }
        var year = info.PublishedYear ?? 0;
        return await CreateAsync(info.Title, year, author.Id, isbn, publisherId);
    }
}
