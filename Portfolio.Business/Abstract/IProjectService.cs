using Portfolio.Domain.Entities;

namespace Portfolio.Business.Abstract;

public interface IProjectService
{
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project> GetProjectByIdAsync(int id);
    Task AddProjectAsync(Project project);
}