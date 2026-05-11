namespace Portfolio.UI.Models;

public class UpdateProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Summary { get; set; }
    public string TechnicalDetail { get; set; }
    public string Tags { get; set; }
}