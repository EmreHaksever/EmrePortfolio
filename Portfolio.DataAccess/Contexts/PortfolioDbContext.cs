using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Entities;

namespace Portfolio.DataAccess.Contexts;

public class PortfolioDbContext : DbContext
{
    // API katmanından gönderilecek bağlantı ayarlarını (Connection String) içeri alıyoruz
    public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options)
    {
    }

    // Veritabanında oluşacak tablolarımız
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<ProfileInfo> ProfileInfos { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Skill> Skills { get; set; }

    // Eğer tablolar oluşurken özel kısıtlamalar (max uzunluk vs.) yapmak istersek burayı kullanırız
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Örnek: Hakkında yazısı uzun olabilir, karakter sınırını kaldıralım
        modelBuilder.Entity<ProfileInfo>()
            .Property(p => p.Summary)
            .HasColumnType("nvarchar(max)");
    }
}