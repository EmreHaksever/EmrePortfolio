namespace Portfolio.UI.Models;

// Listeleme ve Detay için
public class ExperienceDto
{
    public int Id { get; set; }
    public string Title { get; set; } // Örn: Yazılım Geliştirme Stajyeri veya Kurucu
    public string Company { get; set; } // Örn: YAE3D
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } // Devam ediyorsa null olabilir
    public bool IsCurrent { get; set; } // Hala çalışıyor mu?
    public string Location { get; set; } // Bunu ekliyoruz
}

// Yeni Ekleme için
public class CreateExperienceDto
{
    public string Title { get; set; }
    public string Company { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public string Location { get; set; } // Bunu ekliyoruz
}

// Güncelleme için
public class UpdateExperienceDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Company { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public string Location { get; set; } // Bunu ekliyoruz
}