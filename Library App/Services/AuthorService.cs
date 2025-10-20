using Library_App.Data;
using Library_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_App.Services;

public class AuthorService(Library_AppContext db) : IAuthorService
{
    public Task<List<Author>> GetAllAsync() => db.Authors.Include(a=>a.Books).ToListAsync();
    public Task<Author?> GetAsync(int id) => db.Authors.Include(a=>a.Books).FirstOrDefaultAsync(a=>a.Id==id);

    public async Task<Author> GetByNameOrCreateAsync(string name)
    {
        var trimmed = name.Trim();
        var existing = await db.Authors.FirstOrDefaultAsync(a => a.Name == trimmed);
        if (existing is not null) return existing;
        var a = new Author { Name = trimmed };
        db.Authors.Add(a);
        await db.SaveChangesAsync();
        return a;
    }

    public async Task<Author> CreateAsync(string name)
    {
        var a = new Author { Name = name };
        db.Authors.Add(a);
        await db.SaveChangesAsync();
        return a;
    }
}
