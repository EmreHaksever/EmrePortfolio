using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Business.Abstract;
using Portfolio.Business.Concrete;
using Portfolio.DataAccess.Contexts;
using Portfolio.Business.Mapping;
using Portfolio.Domain.Entities; // AppUser için import ettik

// Load .env variables at startup
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envPath))
{
    foreach (var line in File.ReadAllLines(envPath))
    {
        var parts = line.Split('=', 2);
        if (parts.Length == 2)
        {
            var key = parts[0].Trim();
            var val = parts[1].Trim();
            if (!string.IsNullOrEmpty(key) && !key.StartsWith("#"))
            {
                Environment.SetEnvironmentVariable(key, val);
            }
        }
    }
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Veritabanı Bağlantısı (.env dosyasından okuma desteğiyle)
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PortfolioDbContext>(options =>
    options.UseSqlServer(connectionString));

// Sisteme diyoruz ki: "Biri senden IProjectService isterse, ona ProjectManager ver."
// AutoMapper'a "Profil kurallarını bu projedeki Assembly içinde ara" diyoruz
// AutoMapper'ı manuel konfigürasyon ile ekliyoruz
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<Portfolio.Business.Mapping.GeneralMapping>();
});

builder.Services.AddScoped<IExperienceService, ExperienceManager>();
builder.Services.AddScoped<IProjectService, ProjectManager>();
builder.Services.AddScoped<ISkillService, SkillManager>();
builder.Services.AddScoped<IProfileInfoService, ProfileInfoManager>();
builder.Services.AddScoped<IMessageService, MessageManager>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Portfolio API", Version = "v1" });

    // 1. Swagger'a JWT kullanacağımızı tanımlıyoruz
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Lütfen token'ı şu formatta girin: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // 2. Bu güvenliğin tüm kilitli (Authorize) endpointler için geçerli olacağını söylüyoruz
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddScoped<IAuthService, AuthManager>();

// Authentication Ayarları (.env dosyasından okuma desteğiyle)
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll"); // Kimlik doğrulama (Authentication) satırından ÖNCE olmalı!

app.UseAuthentication(); // Önce kimlik doğrula
app.UseAuthorization();  // Sonra yetkilendir

app.UseHttpsRedirection();

app.MapControllers();

// --- OTOMATİK VERİTABANI MİGRASYONU VE ADMIN KULLANICI TOHUMLAMA (SEEDING) ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
    
    try
    {
        // 1. Bekleyen veritabanı migrasyonlarını otomatik olarak uygula
        context.Database.Migrate();

        // 2. .env dosyasındaki admin bilgilerini al, yoksa varsayılanları ata
        var adminUsername = Environment.GetEnvironmentVariable("ADMIN_USERNAME") ?? "emre";
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "Emre12345!";

        // 3. Veritabanında bu kullanıcı var mı diye sorgula
        var adminUser = await context.AppUsers.FirstOrDefaultAsync(u => u.Username == adminUsername);
        if (adminUser == null)
        {
            // Kullanıcı yoksa otomatik ekle
            context.AppUsers.Add(new AppUser
            {
                Username = adminUsername,
                Password = adminPassword
            });
            await context.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        // Migrasyon veya tohumlama sırasında oluşabilecek hataları logla/yazdır
        Console.WriteLine($"[Seeding Error] Veritabanı yapılandırması başarısız oldu: {ex.Message}");
    }
}

app.Run();
