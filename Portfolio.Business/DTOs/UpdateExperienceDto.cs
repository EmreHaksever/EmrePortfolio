namespace Portfolio.Business.DTOs;

public class UpdateExperienceDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Company { get; set; }
    public string Location { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Description { get; set; }
}