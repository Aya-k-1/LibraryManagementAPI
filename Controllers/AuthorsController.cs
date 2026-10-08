using System;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs.Authors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public AuthorsController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
    {
        var authors = await _context.Authors
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName
            })
            .ToListAsync();

        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
    {
        var author = await _context.Authors
            .Where(a => a.Id == id)
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName
            })
            .FirstOrDefaultAsync();

        if (author == null)
        {
            return NotFound();
        }

        return Ok(author);
    }
    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto dto)
    {
        var author = new Author
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        var result = new AuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName
        };

        return CreatedAtAction(
            nameof(GetAuthor),
            new { id = author.Id },
            result
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(
    int id,
    CreateAuthorDto dto)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author == null)
        {
            return NotFound();
        }

        author.FirstName = dto.FirstName;
        author.LastName = dto.LastName;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author == null)
        {
            return NotFound();
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}