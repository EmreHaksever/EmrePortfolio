using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } // Örn: "Docker"
    public string Category { get; set; } // Örn: "Araçlar & DevOps"

}