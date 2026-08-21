namespace LibraryManagementAPI.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public int CategoryId { get; set; }
    // Navigation properties 
    public Category Category { get; set; }
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();
}