using System;
namespace LibraryManagementAPI.DTOs.Books;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? ISBN { get; set; }
    public int CategoryId { get; set; }
}
