namespace LibraryManagementAPI.Models;

public class BookCopy
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string CopyNumber { get; set; }
    public string Condition { get; set; }
    public bool IsAvailable { get; set; }

    // Navigation properties
    public Book Book { get; set; }
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}