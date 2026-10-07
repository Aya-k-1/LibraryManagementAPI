using System;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class BooksController : ControllerBase
{
	private readonly LibraryDbContext _context;

	public BooksController(LibraryDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
	{
		var books = await _context.Books.Select(b=>new BookDto
		{
			Id = b.Id,
			Title = b.Title,
			ISBN = b.ISBN,
			CategoryId = b.CategoryId
		}).ToListAsync();

		return Ok(books);
	}

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetBook(int id)
    {
        var book = await _context.Books
            .Where(b => b.Id == id)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                CategoryId = b.CategoryId
            })
            .FirstOrDefaultAsync();

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }
    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            ISBN = dto.ISBN,
            CategoryId = dto.CategoryId
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        var result = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            ISBN = book.ISBN,
            CategoryId = book.CategoryId
        };

        return CreatedAtAction(
            nameof(GetBook),
            new { id = book.Id },
            result
        );
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, CreateBookDto dto)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        book.Title = dto.Title;
        book.ISBN = dto.ISBN;
        book.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
