namespace LibraryManagementAPI.Models;

public class Loan
{
    public int Id { get; set; }
    public int BookCopyId { get; set; }
    public int MemberId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime DueAt { get; set; }
    public DateTime? ReturnedAt { get; set; }   // ReturnedAt cannn be null(When it's still being borrowed)
    // Navigation Properties
    public BookCopy BookCopy { get; set; }
    public Member Member { get; set; }
}