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
}