using Microsoft.EntityFrameworkCore;
using Portfolio.Business.Abstract;
using Portfolio.DataAccess.Contexts;
using Portfolio.Domain.Entities;

namespace Portfolio.Business.Concrete;

public class ProjectManager : IProjectService
{
    private readonly PortfolioDbContext _context;

    // Dependency Injection: DbContext'i dışarıdan (Program.cs'den) talep ediyoruz.
    public ProjectManager(PortfolioDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        // Tüm projeleri asenkron olarak veritabanından çek.
        return await _context.Projects.ToListAsync();
    }

    public async Task<Project> GetProjectByIdAsync(int id)
    {
        return await _context.Projects.FindAsync(id);
    }

    public async Task AddProjectAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
    }
}