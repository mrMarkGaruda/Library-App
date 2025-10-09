using Library_App.Models;
using Microsoft.AspNetCore.Mvc;
using Library_App.Services;

namespace Library_App.Controllers
{
    [ApiController]
    [Route("books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _books;
        private readonly IAuthorService _authors;
        public BooksController(IBookService books, IAuthorService authors)
        { _books = books; _authors = authors; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAll() => Ok(await _books.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> Get(int id)
        { var b = await _books.GetAsync(id); return b is null ? NotFound() : Ok(b); }

        public record CreateBookDto(string Title, int Year, int AuthorId);

        [HttpPost]
        public async Task<ActionResult<Book>> Create([FromBody] CreateBookDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (await _authors.GetAsync(dto.AuthorId) is null) return BadRequest("AuthorId invalid");
            var b = await _books.CreateAsync(dto.Title, dto.Year, dto.AuthorId);
            return CreatedAtAction(nameof(Get), new { id = b.Id }, b);
        }
    }
}
