using System.Net.Http.Json;
using Portfolio.UI.Models;

namespace Portfolio.UI.Services;

public class ProfileService
{
    private readonly HttpClient _httpClient;
    public ProfileService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<ProfileInfoDto> GetProfileAsync()
    {
        // Genelde tek profil olduğu için API'den ilk kaydı veya 1 nolu ID'yi isteriz
        return await _httpClient.GetFromJsonAsync<ProfileInfoDto>("api/profileinfos");
    }

    public async Task<bool> UpdateProfileAsync(ProfileInfoDto profileDto)
    {
        var response = await _httpClient.PutAsJsonAsync("api/profileinfos", profileDto);
        return response.IsSuccessStatusCode;
    }
}