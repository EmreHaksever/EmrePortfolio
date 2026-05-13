using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;

namespace Portfolio.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SkillsController : ControllerBase
{
    private readonly ISkillService _skillService;

    public SkillsController(ISkillService skillService)
    {
        _skillService = skillService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSkills()
    {
        var skills = await _skillService.GetAllSkillsAsync();
        return Ok(skills);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSkillById(int id)
    {
        var skill = await _skillService.GetSkillByIdAsync(id);

        if (skill == null)
        {
            return NotFound("Yetenek bulunamadı.");
        }

        return Ok(skill);
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddSkill([FromBody] CreateSkillDto createSkillDto)
    {
        await _skillService.AddSkillAsync(createSkillDto);
        return Ok(new { message = "Yetenek başarıyla eklendi." });
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateSkill([FromBody] UpdateSkillDto updateSkillDto)
    {
        await _skillService.UpdateSkillAsync(updateSkillDto);
        return Ok(new { message = "Yetenek başarıyla güncellendi." });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        await _skillService.DeleteSkillAsync(id);
        return Ok(new { message = "Yetenek başarıyla silindi." });
    }
}