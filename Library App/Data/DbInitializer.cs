using Library_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_App.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(Library_AppContext context)
    {
        // Apply any pending migrations (safety if caller forgot)
        await context.Database.MigrateAsync();

        if (await context.Authors.AnyAsync()) return; // Already seeded

        var authors = new List<Author>
        {
            new() { Name = "J. R. R. Tolkien" },
            new() { Name = "George R. R. Martin" },
            new() { Name = "Frank Herbert" }
        };
        context.Authors.AddRange(authors);
        await context.SaveChangesAsync();

        var books = new List<Book>
        {
            new() { Title = "The Hobbit", Year = 1937, AuthorId = authors[0].Id },
            new() { Title = "The Fellowship of the Ring", Year = 1954, AuthorId = authors[0].Id },
            new() { Title = "A Game of Thrones", Year = 1996, AuthorId = authors[1].Id },
            new() { Title = "Dune", Year = 1965, AuthorId = authors[2].Id }
        };
        context.Books.AddRange(books);
        await context.SaveChangesAsync();
    }
}
