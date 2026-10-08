using System;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs.Loans;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public LoansController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoanDto>>> GetLoans()
    {
        var loans = await _context.Loans
            .Select(l => new LoanDto
            {
                Id = l.Id,
                BookCopyId = l.BookCopyId,
                MemberId = l.MemberId,
                BorrowedAt = l.BorrowedAt,
                DueAt = l.DueAt,
                ReturnedAt = l.ReturnedAt
            })
            .ToListAsync();

        return Ok(loans);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LoanDto>> GetLoan(int id)
    {
        var loan = await _context.Loans
            .Where(l => l.Id == id)
            .Select(l => new LoanDto
            {
                Id = l.Id,
                BookCopyId = l.BookCopyId,
                MemberId = l.MemberId,
                BorrowedAt = l.BorrowedAt,
                DueAt = l.DueAt,
                ReturnedAt = l.ReturnedAt
            })
            .FirstOrDefaultAsync();

        if (loan == null)
        {
            return NotFound();
        }

        return Ok(loan);
    }

    [HttpPost]
    public async Task<ActionResult<LoanDto>> CreateLoan(
        CreateLoanDto dto)
    {
        var bookCopy = await _context.BookCopies
            .FindAsync(dto.BookCopyId);

        if (bookCopy == null)
        {
            return BadRequest("The specified book copy does not exist.");
        }

        if (!bookCopy.IsAvailable)
        {
            return Conflict("This book copy is already borrowed.");
        }

        var member = await _context.Members
            .FindAsync(dto.MemberId);

        if (member == null)
        {
            return BadRequest("The specified member does not exist.");
        }

        var loan = new Loan
        {
            BookCopyId = dto.BookCopyId,
            MemberId = dto.MemberId,
            BorrowedAt = DateTime.Now,
            DueAt = dto.DueAt
        };

        bookCopy.IsAvailable = false;

        _context.Loans.Add(loan);

        await _context.SaveChangesAsync();

        var result = new LoanDto
        {
            Id = loan.Id,
            BookCopyId = loan.BookCopyId,
            MemberId = loan.MemberId,
            BorrowedAt = loan.BorrowedAt,
            DueAt = loan.DueAt,
            ReturnedAt = loan.ReturnedAt
        };

        return CreatedAtAction(
            nameof(GetLoan),
            new { id = loan.Id },
            result
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLoan(
        int id,
        CreateLoanDto dto)
    {
        var loan = await _context.Loans.FindAsync(id);

        if (loan == null)
        {
            return NotFound();
        }

        var bookCopy = await _context.BookCopies
            .FindAsync(dto.BookCopyId);

        if (bookCopy == null)
        {
            return BadRequest("The specified book copy does not exist.");
        }

        var member = await _context.Members
            .FindAsync(dto.MemberId);

        if (member == null)
        {
            return BadRequest("The specified member does not exist.");
        }

        loan.BookCopyId = dto.BookCopyId;
        loan.MemberId = dto.MemberId;
        loan.DueAt = dto.DueAt;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLoan(int id)
    {
        var loan = await _context.Loans.FindAsync(id);

        if (loan == null)
        {
            return NotFound();
        }

        _context.Loans.Remove(loan);

        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpPost("{id}/return")]
    public async Task<IActionResult> ReturnLoan(int id)
    {
        var loan = await _context.Loans
            .Include(l => l.BookCopy)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (loan == null)
        {
            return NotFound();
        }

        if (loan.ReturnedAt != null)
        {
            return Conflict("This loan has already been returned.");
        }

        loan.ReturnedAt = DateTime.Now;
        loan.BookCopy.IsAvailable = true;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
