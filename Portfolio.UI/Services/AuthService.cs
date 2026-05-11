using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization; // Bunu ekle
using Portfolio.UI.Models;

namespace Portfolio.UI.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider; // Bunu ekle

    public AuthService(HttpClient httpClient,
                       ILocalStorageService localStorage,
                       AuthenticationStateProvider authStateProvider) // Inject et
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

            await _localStorage.SetItemAsync("authToken", result.Token);

            // KRİTİK NOKTA: Blazor'a giriş yapıldığını haber veriyoruz
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);

            return true;
        }

        return false;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");

        // Çıkış yapıldığını sisteme haber ver
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }
}