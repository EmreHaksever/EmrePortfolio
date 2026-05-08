using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities;

public class ProfileInfo
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Title { get; set; } // Örn: "Bilgisayar Mühendisliği Öğrencisi"
    public string Summary { get; set; } // "Karmaşık problemlere..." yazısı
    public decimal Gpa { get; set; } // 3.31 gibi
    public string Email { get; set; }
    public string Phone { get; set; }
    public string GithubUrl { get; set; }
    public string LinkedInUrl { get; set; }
}