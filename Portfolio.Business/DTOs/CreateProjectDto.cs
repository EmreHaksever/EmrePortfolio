namespace Portfolio.Business.DTOs;

public class CreateProjectDto
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string TechnicalDetail { get; set; }
    public string Tags { get; set; }
}