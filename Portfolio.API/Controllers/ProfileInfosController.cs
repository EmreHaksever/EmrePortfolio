using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;

namespace Portfolio.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileInfosController : ControllerBase
{
    private readonly IProfileInfoService _profileInfoService;

    public ProfileInfosController(IProfileInfoService profileInfoService)
    {
        _profileInfoService = profileInfoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfileInfo()
    {
        var profileInfo = await _profileInfoService.GetProfileInfoAsync();
        return Ok(profileInfo);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddProfileInfo([FromBody] CreateProfileInfoDto createProfileInfoDto)
    {
        await _profileInfoService.AddProfileInfoAsync(createProfileInfoDto);
        return Ok(new { message = "Profil bilgisi başarıyla eklendi." });
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateProfileInfo([FromBody] ProfileInfoDto updateProfileInfoDto)
    {
        await _profileInfoService.UpdateProfileInfoAsync(updateProfileInfoDto);
        return Ok(new { message = "Profil bilgileri başarıyla güncellendi." });
    }
}