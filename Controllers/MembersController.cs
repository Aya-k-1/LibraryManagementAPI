using System;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs.Members;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly LibraryDbContext _context;
    public MembersController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers()
    {
        var members = await _context.Members
            .Select(m => new MemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                JoinedAt = m.JoinedAt
            })
            .ToListAsync();
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto>> GetMember(int id)
    {
        var member = await _context.Members
            .Where(m => m.Id == id)
            .Select(m => new MemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                JoinedAt = m.JoinedAt
            })
            .FirstOrDefaultAsync();

        if (member == null)
        {
            return NotFound();
        }

        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> CreateMember(
        CreateMemberDto dto)
    {
        var existingMember = await _context.Members
            .AnyAsync(m => m.Email == dto.Email);

        if (existingMember)
        {
            return Conflict("A member with this email already exists.");
        }

        var member = new Member
        {
            FullName = dto.FullName,
            Email = dto.Email
        };

        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        var result = new MemberDto
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            JoinedAt = member.JoinedAt
        };

        return CreatedAtAction(
            nameof(GetMember),
            new { id = member.Id },
            result
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMember(
        int id,
        CreateMemberDto dto)
    {
        var member = await _context.Members.FindAsync(id);

        if (member == null)
        {
            return NotFound();
        }

        var emailAlreadyUsed = await _context.Members
            .AnyAsync(m => m.Email == dto.Email && m.Id != id);

        if (emailAlreadyUsed)
        {
            return Conflict("A member with this email already exists.");
        }

        member.FullName = dto.FullName;
        member.Email = dto.Email;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        var member = await _context.Members.FindAsync(id);

        if (member == null)
        {
            return NotFound();
        }

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}