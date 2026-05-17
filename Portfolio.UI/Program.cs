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

// 2. HttpClient Factory'i kuruyoruz ve ApiBaseUrl değerini dinamik olarak okuyoruz
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7032/";

builder.Services.AddHttpClient("PortfolioAPI", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<JwtAuthorizationHandler>();

// 3. Sistem "Bana HttpClient ver" dediğinde, kuryeli olan bu özel Client'ı teslim et diyoruz
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("PortfolioAPI"));

// 4. Diğer Servisler
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<ExperienceService>();
builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<MessageService>();

await builder.Build().RunAsync();
