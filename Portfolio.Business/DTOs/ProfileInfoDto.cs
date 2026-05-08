namespace Portfolio.Business.DTOs;

public class ProfileInfoDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public decimal Gpa { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string GithubUrl { get; set; }
    public string LinkedInUrl { get; set; }
}