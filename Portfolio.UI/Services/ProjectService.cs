using System.Net.Http.Json;
using Portfolio.UI.Models;

namespace Portfolio.UI.Services;

public class ProjectService
{
    private readonly HttpClient _httpClient;

    public ProjectService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        // Backend'den projeleri çekiyoruz
        return await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects");
    }

    // Yeni Proje Ekleme
    public async Task<bool> CreateProjectAsync(CreateProjectDto createProjectDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/projects", createProjectDto);
        return response.IsSuccessStatusCode;
    }

    // Proje Silme
    public async Task<bool> DeleteProjectAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/projects/{id}");
        return response.IsSuccessStatusCode;
    }
}