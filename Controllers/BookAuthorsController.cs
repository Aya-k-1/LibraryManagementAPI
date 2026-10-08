using System;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs.BookAuthors;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookAuthorsController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public BookAuthorsController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddAuthorToBook(
        CreateBookAuthorDto dto)
    {
        var book = await _context.Books.FindAsync(dto.BookId);

        if (book == null)
        {
            return BadRequest("The specified book does not exist.");
        }

        var author = await _context.Authors.FindAsync(dto.AuthorId);

        if (author == null)
        {
            return BadRequest("The specified author does not exist.");
        }

        var relationshipExists = await _context.BookAuthors
            .AnyAsync(ba =>
                ba.BookId == dto.BookId &&
                ba.AuthorId == dto.AuthorId);

        if (relationshipExists)
        {
            return Conflict("This author is already associated with this book.");
        }

        var bookAuthor = new BookAuthor
        {
            BookId = dto.BookId,
            AuthorId = dto.AuthorId
        };

        _context.BookAuthors.Add(bookAuthor);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}