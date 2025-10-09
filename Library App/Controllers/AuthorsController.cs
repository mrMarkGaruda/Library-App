using Library_App.Models;
using Microsoft.AspNetCore.Mvc;
using Library_App.Services;

namespace Library_App.Controllers;

[ApiController]
[Route("authors")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authors;
    public AuthorsController(IAuthorService authors) => _authors = authors;

    // GET /authors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAll() => Ok(await _authors.GetAllAsync());

    // GET /authors/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Author>> Get(int id)
    { var a = await _authors.GetAsync(id); return a is null ? NotFound() : Ok(a); }

    public record CreateAuthorDto(string Name);

    // POST /authors
    [HttpPost]
    public async Task<ActionResult<Author>> Create(CreateAuthorDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name required");
        var a = await _authors.CreateAsync(dto.Name);
        return CreatedAtAction(nameof(Get), new { id = a.Id }, a);
    }
}
