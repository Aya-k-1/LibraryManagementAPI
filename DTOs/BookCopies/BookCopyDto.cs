using System;
namespace LibraryManagementAPI.DTOs.BookCopies;

public class BookCopyDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string CopyNumber { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}