using Microsoft.AspNetCore.Mvc;
using Portfolio.Business.Abstract;
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
    public async Task<IActionResult> AddProject([FromBody] Project project)
    {
        // Gelen veri boş mu kontrolü
        if (project == null)
        {
            return BadRequest("Proje verisi boş olamaz.");
        }

        await _projectService.AddProjectAsync(project);

        // Başarılı olursa 201 Created döner ve eklenen projeyi gösterir
        return CreatedAtAction(nameof(GetAllProjects), new { id = project.Id }, project);
    }
}