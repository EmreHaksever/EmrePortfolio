namespace Portfolio.UI.Models;



public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }            // Name (Entity ile aynı)
    public string Summary { get; set; }         // Summary (Entity ile aynı)
    public string TechnicalDetail { get; set; } // Teknik Detay
    public string Tags { get; set; }            // Etiketler
}

public class CreateProjectDto
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string TechnicalDetail { get; set; }
    public string Tags { get; set; }
}