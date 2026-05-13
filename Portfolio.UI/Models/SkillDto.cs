namespace Portfolio.UI.Models;

public class SkillDto
{
    public int Id { get; set; }
    public string Name { get; set; } // Örn: C# & .NET, React Native, 3D İmalat
    public string Category { get; set; } // Bunu ekliyoruz (Backend'de tipinin string olduğunu varsayıyorum)
}

public class CreateSkillDto
{
    public string Name { get; set; }
    public string Category { get; set; } // Bunu ekliyoruz (Backend'de tipinin string olduğunu varsayıyorum)
}

public class UpdateSkillDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; } // Bunu ekliyoruz (Backend'de tipinin string olduğunu varsayıyorum)
}