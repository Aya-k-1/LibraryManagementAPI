using System;
namespace LibraryManagementAPI.DTOs.Loans;

public class CreateLoanDto
{
    public int BookCopyId { get; set; }
    public int MemberId { get; set; }
    public DateTime DueAt { get; set; }
}