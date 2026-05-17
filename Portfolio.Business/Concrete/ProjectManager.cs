using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;
using Portfolio.DataAccess.Contexts;
using Portfolio.Domain.Entities;


namespace Portfolio.Business.Concrete;

public class ProjectManager : IProjectService
{
    private readonly PortfolioDbContext _context;
    private readonly IMapper _mapper; // AutoMapper'ı tanımlıyoruz

    public ProjectManager(PortfolioDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper; // Gelen nesneyi field'a atıyoruz
    }

    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _context.Projects.OrderBy(p => p.SortOrder).ToListAsync();

        // Entity listesini DTO listesine çeviriyoruz (Mapping)
        var projectDtos = projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Summary = p.Summary,
            TechnicalDetail = p.TechnicalDetail,
            Tags = p.Tags,
            SortOrder = p.SortOrder
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
            Tags = project.Tags,
            SortOrder = project.SortOrder
        };
    }

    public async Task UpdateProjectAsync(UpdateProjectDto updateProjectDto)
    {
        // Gelen DTO'yu Entity'ye çevir ve güncelle
        var project = _mapper.Map<Project>(updateProjectDto);
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(int id)
    {
        // Önce veritabanında bu id'ye sahip proje var mı diye bul, varsa sil
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddProjectAsync(CreateProjectDto createProjectDto)
    {
        var maxOrder = await _context.Projects.AnyAsync() 
            ? await _context.Projects.MaxAsync(p => p.SortOrder) 
            : 0;

        // Dışarıdan gelen DTO'yu veritabanı nesnesine (Entity) çeviriyoruz
        var newProject = new Project
        {
            Name = createProjectDto.Name,
            Summary = createProjectDto.Summary,
            TechnicalDetail = createProjectDto.TechnicalDetail,
            Tags = createProjectDto.Tags,
            SortOrder = maxOrder + 1
        };

        await _context.Projects.AddAsync(newProject);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProjectOrdersAsync(List<int> sortedIds)
    {
        if (sortedIds == null || !sortedIds.Any()) return;

        for (int i = 0; i < sortedIds.Count; i++)
        {
            var id = sortedIds[i];
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.SortOrder = i + 1;
                _context.Projects.Update(project);
            }
        }
        await _context.SaveChangesAsync();
    }
}