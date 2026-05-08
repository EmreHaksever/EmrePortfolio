using Portfolio.Business.DTOs;

namespace Portfolio.Business.Abstract;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto> GetProjectByIdAsync(int id);
    Task AddProjectAsync(CreateProjectDto createProjectDto);
}