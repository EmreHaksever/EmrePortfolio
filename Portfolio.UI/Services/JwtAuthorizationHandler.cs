using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace Portfolio.UI.Services;

public class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public JwtAuthorizationHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Local Storage'dan token'ı al
        var token = await _localStorage.GetItemAsync<string>("authToken");

        // Eğer token varsa, isteğin içine "Bearer" formatında ekle
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // İsteği API'ye yola çıkar
        return await base.SendAsync(request, cancellationToken);
    }
}