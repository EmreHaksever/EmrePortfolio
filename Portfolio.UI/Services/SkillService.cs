using System.Net.Http.Json;
using Portfolio.UI.Models;

namespace Portfolio.UI.Services;

public class SkillService
{
    private readonly HttpClient _httpClient;

    public SkillService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SkillDto>> GetAllSkillsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<SkillDto>>("api/skills");
    }

    public async Task<SkillDto> GetSkillByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SkillDto>($"api/skills/{id}");
    }

    public async Task<bool> CreateSkillAsync(CreateSkillDto createSkillDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/skills", createSkillDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateSkillAsync(UpdateSkillDto updateSkillDto)
    {
        var response = await _httpClient.PutAsJsonAsync("api/skills", updateSkillDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteSkillAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/skills/{id}");
        return response.IsSuccessStatusCode;
    }
}