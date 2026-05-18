using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Portfolio.DataAccess.Contexts;
using Portfolio.Domain.Entities;

Console.WriteLine("Starting data import to PostgreSQL...");

var optionsBuilder = new DbContextOptionsBuilder<PortfolioDbContext>();
optionsBuilder.UseNpgsql("Host=localhost;Database=EmrePortfolioDb;Username=postgres;Password=YourSecurePassword123!");

using var context = new PortfolioDbContext(optionsBuilder.Options);

var jsonString = await File.ReadAllTextAsync("../data_backup.json");
var backup = JsonSerializer.Deserialize<BackupData>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

if (backup != null)
{
    // Enable identity insert or just insert (PostgreSQL handles sequence correctly if we reset it, but since we are inserting with IDs, we should use context.Database.ExecuteSqlRaw or just let EF insert and then fix sequences)
    // Actually, EF Core with Npgsql allows inserting explicit IDs for serial columns, but you might need to fix sequences afterwards.
    
    // Clear existing data (if any)
    if (!await context.AppUsers.AnyAsync())
    {
        await context.AppUsers.AddRangeAsync(backup.AppUsers);
        await context.ProfileInfos.AddRangeAsync(backup.ProfileInfos);
        await context.Experiences.AddRangeAsync(backup.Experiences);
        await context.Projects.AddRangeAsync(backup.Projects);
        await context.Skills.AddRangeAsync(backup.Skills);
        await context.Messages.AddRangeAsync(backup.Messages);
        
        await context.SaveChangesAsync();
        Console.WriteLine("Data imported successfully!");
        
        // Reset sequences
        Console.WriteLine("Fixing sequences...");
        await FixSequence(context, "AppUsers", backup.AppUsers.Max(x => (int?)x.Id) ?? 0);
        await FixSequence(context, "ProfileInfos", backup.ProfileInfos.Max(x => (int?)x.Id) ?? 0);
        await FixSequence(context, "Experiences", backup.Experiences.Max(x => (int?)x.Id) ?? 0);
        await FixSequence(context, "Projects", backup.Projects.Max(x => (int?)x.Id) ?? 0);
        await FixSequence(context, "Skills", backup.Skills.Max(x => (int?)x.Id) ?? 0);
        await FixSequence(context, "Messages", backup.Messages.Max(x => (int?)x.Id) ?? 0);
        Console.WriteLine("Sequences fixed.");
    }
    else
    {
        Console.WriteLine("Database is not empty, skipping import.");
    }
}

async Task FixSequence(PortfolioDbContext ctx, string tableName, int maxId)
{
    if (maxId > 0)
    {
        var command = $"SELECT setval('\"{tableName}_Id_seq\"', {maxId});";
        try { await ctx.Database.ExecuteSqlRawAsync(command); } catch { /* Ignore if seq doesn't exist */ }
    }
}

public class BackupData
{
    public List<AppUser> AppUsers { get; set; } = new();
    public List<ProfileInfo> ProfileInfos { get; set; } = new();
    public List<Experience> Experiences { get; set; } = new();
    public List<Project> Projects { get; set; } = new();
    public List<Skill> Skills { get; set; } = new();
    public List<Message> Messages { get; set; } = new();
}
