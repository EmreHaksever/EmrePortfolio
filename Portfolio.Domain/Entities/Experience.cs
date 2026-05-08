using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities;

public class Experience
{
    public int Id { get; set; }
    public string Title { get; set; } // Örn: "Yazılım Geliştirme Stajyeri"
    public string Company { get; set; }
    public string Location { get; set; }
    public string StartDate { get; set; } // Örn: "07.2024"
    public string EndDate { get; set; } // Örn: "08.2024" veya "Devam Ediyor"
    public string Description { get; set; } // Yaptığın işin maddeli detayları
}
