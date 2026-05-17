using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } // Örn: "MiniERP Sistemi"
    public string Summary { get; set; } // Proje Özeti
    public string TechnicalDetail { get; set; } // Teknik Detay
    public string Tags { get; set; } // Ekranda etiket olarak göstermek için (Örn: ".NET, MSSQL, EF Core")
    public int SortOrder { get; set; }
}