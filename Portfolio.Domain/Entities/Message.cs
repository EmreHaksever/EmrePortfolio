namespace Portfolio.Domain.Entities;

public class Message
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public DateTime DateSent { get; set; }
    public bool IsRead { get; set; }
}