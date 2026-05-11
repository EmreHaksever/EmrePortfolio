using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portfolio.UI;
using Portfolio.UI.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Kuryemizi sisteme kaydediyoruz
builder.Services.AddTransient<JwtAuthorizationHandler>();

// 2. HttpClient Factory'i kuruyoruz ve Kuryeyi (Handler) araya sokuyoruz
builder.Services.AddHttpClient("PortfolioAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7032/"); // Kendi portun
}).AddHttpMessageHandler<JwtAuthorizationHandler>();

// 3. Sistem "Bana HttpClient ver" dediðinde, kuryeli olan bu özel Client'ý teslim et diyoruz
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("PortfolioAPI"));

// 4. Diðer Servisler
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();