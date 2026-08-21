namespace LibraryManagementAPI.Models;

public class Member
{
	public int Id { get; set; }
	public string FullName { get; set; }
	public string Email {  get; set; }
	public DateTime JoinedAt { get; set; }
	//Navigation Property
	public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
