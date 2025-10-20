namespace Library_App.Services;

public record GoogleBookInfo(string Title, List<string> Authors, string? Publisher, int? PublishedYear);

public interface IGoogleBooksService
{
    Task<GoogleBookInfo?> GetByIsbnAsync(string isbn);
}
