using System;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs.BookCopies;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookCopiesController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public BookCopiesController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookCopyDto>>> GetBookCopies()
    {
        var copies = await _context.BookCopies
            .Select(bc => new BookCopyDto
            {
                Id = bc.Id,
                BookId = bc.BookId,
                CopyNumber = bc.CopyNumber,
                Condition = bc.Condition,
                IsAvailable = bc.IsAvailable
            })
            .ToListAsync();

        return Ok(copies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookCopyDto>> GetBookCopy(int id)
    {
        var copy = await _context.BookCopies
            .Where(bc => bc.Id == id)
            .Select(bc => new BookCopyDto
            {
                Id = bc.Id,
                BookId = bc.BookId,
                CopyNumber = bc.CopyNumber,
                Condition = bc.Condition,
                IsAvailable = bc.IsAvailable
            })
            .FirstOrDefaultAsync();

        if (copy == null)
        {
            return NotFound();
        }

        return Ok(copy);
    }

    [HttpPost]
    public async Task<ActionResult<BookCopyDto>> CreateBookCopy(
        CreateBookCopyDto dto)
    {
        var book = await _context.Books.FindAsync(dto.BookId);

        if (book == null)
        {
            return BadRequest("The specified book does not exist.");
        }

        var copy = new BookCopy
        {
            BookId = dto.BookId,
            CopyNumber = dto.CopyNumber,
            Condition = dto.Condition,
            IsAvailable = dto.IsAvailable
        };

        _context.BookCopies.Add(copy);
        await _context.SaveChangesAsync();

        var result = new BookCopyDto
        {
            Id = copy.Id,
            BookId = copy.BookId,
            CopyNumber = copy.CopyNumber,
            Condition = copy.Condition,
            IsAvailable = copy.IsAvailable
        };

        return CreatedAtAction(
            nameof(GetBookCopy),
            new { id = copy.Id },
            result
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBookCopy(
        int id,
        CreateBookCopyDto dto)
    {
        var copy = await _context.BookCopies.FindAsync(id);

        if (copy == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FindAsync(dto.BookId);

        if (book == null)
        {
            return BadRequest("The specified book does not exist.");
        }

        copy.BookId = dto.BookId;
        copy.CopyNumber = dto.CopyNumber;
        copy.Condition = dto.Condition;
        copy.IsAvailable = dto.IsAvailable;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBookCopy(int id)
    {
        var copy = await _context.BookCopies.FindAsync(id);

        if (copy == null)
        {
            return NotFound();
        }

        _context.BookCopies.Remove(copy);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}