using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portfolio.UI;
using Blazored.LocalStorage; // Bunu ekliyoruz

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient ayarý: API'nin adresini buraya veriyoruz
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5018/") });

// LocalStorage servisini sisteme tanýtýyoruz
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();