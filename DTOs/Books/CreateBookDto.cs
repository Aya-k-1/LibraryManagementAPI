using System;
namespace LibraryManagementAPI.DTOs.Books;

public class CreateBookDto
{
	public string Title { get; set; } = string.Empty;

	public string? ISBN { get; set; }

	public int CategoryId { get; set; }
}
