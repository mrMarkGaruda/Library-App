using Library_App.Data;
using Library_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_App.Services;

public class PublisherService(Library_AppContext db) : IPublisherService
{
    public async Task<Publisher> GetByNameOrCreateAsync(string name)
    {
        var trimmed = name.Trim();
        var existing = await db.Publishers.FirstOrDefaultAsync(p => p.Name == trimmed);
        if (existing is not null) return existing;
        var p = new Publisher { Name = trimmed };
        db.Publishers.Add(p);
        await db.SaveChangesAsync();
        return p;
    }
}
