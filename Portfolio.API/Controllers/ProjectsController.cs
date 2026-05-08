using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;
using Portfolio.Domain.Entities;

namespace Portfolio.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    // Dependency Injection ile servisimizi alıyoruz
    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // GET: api/projects
    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    // POST: api/projects
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddProject([FromBody] CreateProjectDto createProjectDto)
    {
        if (createProjectDto == null)
        {
            return BadRequest("Proje verisi boş olamaz.");
        }

        await _projectService.AddProjectAsync(createProjectDto);

        return Ok(new { message = "Proje başarıyla eklendi." });
    }

    // Başarılı olursa 201 Created döner ve eklenen projeyi gösterir.

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectDto updateProjectDto)
    {
        await _projectService.UpdateProjectAsync(updateProjectDto);
        return Ok(new { message = "Proje başarıyla güncellendi." });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteProject(int id)
    {
        await _projectService.DeleteProjectAsync(id);
        return Ok(new { message = "Proje başarıyla silindi." });
    }
}