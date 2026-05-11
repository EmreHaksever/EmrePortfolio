using System.Net.Http.Json;
using Blazored.LocalStorage;
using Portfolio.UI.Models;

namespace Portfolio.UI.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<bool> LoginAsync(LoginDto loginDto)
    {
        // Backend'e POST isteği atıyoruz
        var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            // Giriş başarılıysa token'ı al
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

            // Token'ı tarayıcının Local Storage'ına kaydet
            await _localStorage.SetItemAsync("authToken", result.Token);
            return true;
        }

        // Başarısızsa false dön
        return false;
    }
}