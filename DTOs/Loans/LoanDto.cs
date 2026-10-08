using System;
namespace LibraryManagementAPI.DTOs.Loans;

public class LoanDto
{
    public int Id { get; set; }
    public int BookCopyId { get; set; }
    public int MemberId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime DueAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
}