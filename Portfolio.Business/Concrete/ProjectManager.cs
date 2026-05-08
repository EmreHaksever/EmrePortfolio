using Microsoft.EntityFrameworkCore;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;
using Portfolio.DataAccess.Contexts;
using Portfolio.Domain.Entities;

namespace Portfolio.Business.Concrete;

public class ProjectManager : IProjectService
{
    private readonly PortfolioDbContext _context;

    public ProjectManager(PortfolioDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _context.Projects.ToListAsync();

        // Entity listesini DTO listesine çeviriyoruz (Mapping)
        var projectDtos = projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Summary = p.Summary,
            TechnicalDetail = p.TechnicalDetail,
            Tags = p.Tags
        }).ToList();

        return projectDtos;
    }

    public async Task<ProjectDto> GetProjectByIdAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return null;

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Summary = project.Summary,
            TechnicalDetail = project.TechnicalDetail,
            Tags = project.Tags
        };
    }

    public async Task AddProjectAsync(CreateProjectDto createProjectDto)
    {
        // Dışarıdan gelen DTO'yu veritabanı nesnesine (Entity) çeviriyoruz
        var newProject = new Project
        {
            Name = createProjectDto.Name,
            Summary = createProjectDto.Summary,
            TechnicalDetail = createProjectDto.TechnicalDetail,
            Tags = createProjectDto.Tags
        };

        await _context.Projects.AddAsync(newProject);
        await _context.SaveChangesAsync();
    }
}