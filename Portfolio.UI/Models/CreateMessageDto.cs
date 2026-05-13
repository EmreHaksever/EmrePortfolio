namespace Portfolio.UI.Models;

public class CreateMessageDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}