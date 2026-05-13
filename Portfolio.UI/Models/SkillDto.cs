namespace Portfolio.UI.Models;

public class SkillDto
{
    public int Id { get; set; }
    public string Name { get; set; } // Örn: C# & .NET, React Native, 3D İmalat
    public byte Level { get; set; }  // Örn: 85 (Yüzdelik dilim için)
}

public class CreateSkillDto
{
    public string Name { get; set; }
    public byte Level { get; set; }
}

public class UpdateSkillDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public byte Level { get; set; }
}