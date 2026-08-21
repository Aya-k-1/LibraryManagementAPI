namespace LibraryManagementAPI.Models;

public class Author
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    //Navigation Porperty
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}