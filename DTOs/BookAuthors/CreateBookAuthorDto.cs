using System;
namespace LibraryManagementAPI.DTOs.BookAuthors;

public class CreateBookAuthorDto
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
}