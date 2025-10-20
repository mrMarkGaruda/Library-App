using System.Net.Http.Json;
using System.Text.Json;

namespace Library_App.Services;

public class GoogleBooksService(HttpClient http)
    : IGoogleBooksService
{
    private const string BaseUrl = "https://www.googleapis.com/books/v1/volumes?q=isbn:";

    public async Task<GoogleBookInfo?> GetByIsbnAsync(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn)) return null;
        var url = BaseUrl + Uri.EscapeDataString(isbn);
        using var resp = await http.GetAsync(url);
        if (!resp.IsSuccessStatusCode) return null;
        using var stream = await resp.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        if (!doc.RootElement.TryGetProperty("totalItems", out var total) || total.GetInt32() <= 0)
            return null;
        var item = doc.RootElement.GetProperty("items")[0];
        var vi = item.GetProperty("volumeInfo");
        var title = vi.GetProperty("title").GetString() ?? "";
        var authors = new List<string>();
        if (vi.TryGetProperty("authors", out var aArr) && aArr.ValueKind == JsonValueKind.Array)
        {
            foreach (var a in aArr.EnumerateArray()) authors.Add(a.GetString() ?? "");
        }
        string? publisher = vi.TryGetProperty("publisher", out var pub) ? pub.GetString() : null;
        int? year = null;
        if (vi.TryGetProperty("publishedDate", out var pd))
        {
            var s = pd.GetString();
            if (!string.IsNullOrEmpty(s))
            {
                var digits = new string(s.Where(char.IsDigit).ToArray());
                if (digits.Length >= 4 && int.TryParse(digits.AsSpan(0,4), out var y)) year = y;
            }
        }
        return new GoogleBookInfo(title, authors, publisher, year);
    }
}
