using System.Net.Http.Json;
using Portfolio.UI.Models;

namespace Portfolio.UI.Services;

public class ExperienceService
{
    private readonly HttpClient _httpClient;

    public ExperienceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ExperienceDto>> GetAllExperiencesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ExperienceDto>>("api/experiences");
    }

    public async Task<ExperienceDto> GetExperienceByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ExperienceDto>($"api/experiences/{id}");
    }

    public async Task<bool> CreateExperienceAsync(CreateExperienceDto createExperienceDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/experiences", createExperienceDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateExperienceAsync(UpdateExperienceDto updateExperienceDto)
    {
        var response = await _httpClient.PutAsJsonAsync("api/experiences", updateExperienceDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteExperienceAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/experiences/{id}");
        return response.IsSuccessStatusCode;
    }
}