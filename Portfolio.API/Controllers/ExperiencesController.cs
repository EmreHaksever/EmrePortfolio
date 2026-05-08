using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;

namespace Portfolio.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExperiencesController : ControllerBase
{
    private readonly IExperienceService _experienceService;

    public ExperiencesController(IExperienceService experienceService)
    {
        _experienceService = experienceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExperiences()
    {
        var experiences = await _experienceService.GetAllExperiencesAsync();
        return Ok(experiences);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddExperience([FromBody] CreateExperienceDto createExperienceDto)
    {
        if (createExperienceDto == null)
        {
            return BadRequest("Deneyim verisi boş olamaz.");
        }

        await _experienceService.AddExperienceAsync(createExperienceDto);

        return Ok(new { message = "Deneyim başarıyla eklendi." });
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateExperience([FromBody] UpdateExperienceDto updateExperienceDto)
    {
        await _experienceService.UpdateExperienceAsync(updateExperienceDto);
        return Ok(new { message = "Deneyim güncellendi." });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteExperience(int id)
    {
        await _experienceService.DeleteExperienceAsync(id);
        return Ok(new { message = "Deneyim silindi." });
    }
}