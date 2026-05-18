using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Entities;


namespace Portfolio.DataAccess.Contexts;

// Sınıfın public olduğundan emin oluyoruz (Erişilebilirlik hatasını önlemek için)
public class PortfolioDbContext : DbContext
{
    public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options)
    {
    }

    // Veritabanı tabloları
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<ProfileInfo> ProfileInfos { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Message> Messages { get; set; } // Bu satırın burada olması şart

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Summary alanı için yaptığımızın aynısını GPA için yapıyoruz
        modelBuilder.Entity<ProfileInfo>()
            .Property(p => p.Gpa)
            .HasColumnType("decimal(3,2)"); // Toplam 3 basamak, virgülden sonra 2 basamak (Örn: 4.00)

        modelBuilder.Entity<ProfileInfo>()
            .Property(p => p.Summary)
            .HasColumnType("text");

        // PostgreSQL'de DateTime tipini SQL Server'daki gibi (timezone bilgisi olmadan) 'timestamp without time zone' olarak eşle
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));
            foreach (var property in properties)
            {
                property.SetColumnType("timestamp without time zone");
            }
        }
    }
}