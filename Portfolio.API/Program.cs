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
    options.UseNpgsql(connectionString));

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
        // 1. Bekleyen veritabanı migrasyonlarını otomatik olarak uygula (Retry mekanizmalı)
        int retryCount = 0;
        int maxRetry = 6;
        bool migrationSucceeded = false;
        
        while (!migrationSucceeded && retryCount < maxRetry)
        {
            try
            {
                context.Database.Migrate();
                migrationSucceeded = true;
                Console.WriteLine("[Database] Migrasyonlar başarıyla uygulandı.");
            }
            catch (Exception ex)
            {
                retryCount++;
                Console.WriteLine($"[Database] Veritabanına bağlanılamadı. Yeniden deneniyor ({retryCount}/{maxRetry})... Hata: {ex.Message}");
                if (retryCount >= maxRetry)
                {
                    throw; // Maksimum denemeye ulaşıldıysa yukarıdaki ana catch bloğuna fırlat
                }
                System.Threading.Thread.Sleep(3000); // 3 saniye bekle
            }
        }

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

        // 4. Eski MSSQL verilerini (data_backup.json) içeri aktar
        var backupPath = Path.Combine(AppContext.BaseDirectory, "data_backup.json");
        if (File.Exists(backupPath) && !await context.Experiences.AnyAsync())
        {
            var jsonString = await File.ReadAllTextAsync(backupPath);
            var backupOptions = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            // AppUser sınıfı için bir anonim tip kullanamayız çünkü Domain nesnesi bekliyor.
            // Fakat AppUser dışında kalan tüm verileri ekleyebiliriz (Zaten admin eklendi).
            // BackupData sınıfını burada geçici bir anonymous veya record ile okuyamayız,
            // JSON içerisindeki dizileri dinamik olarak okumak daha güvenli:
            using var doc = System.Text.Json.JsonDocument.Parse(jsonString);
            var root = doc.RootElement;
            
            if (root.TryGetProperty("AppUsers", out var appUsersEl) && !await context.AppUsers.AnyAsync())
            {
                var appUsers = System.Text.Json.JsonSerializer.Deserialize<List<AppUser>>(appUsersEl.GetRawText(), backupOptions);
                if (appUsers != null) await context.AppUsers.AddRangeAsync(appUsers);
            }
            if (root.TryGetProperty("ProfileInfos", out var profilesEl))
            {
                var profiles = System.Text.Json.JsonSerializer.Deserialize<List<ProfileInfo>>(profilesEl.GetRawText(), backupOptions);
                if (profiles != null) await context.ProfileInfos.AddRangeAsync(profiles);
            }
            if (root.TryGetProperty("Experiences", out var experiencesEl))
            {
                var experiences = System.Text.Json.JsonSerializer.Deserialize<List<Experience>>(experiencesEl.GetRawText(), backupOptions);
                if (experiences != null) await context.Experiences.AddRangeAsync(experiences);
            }
            if (root.TryGetProperty("Projects", out var projectsEl))
            {
                var projects = System.Text.Json.JsonSerializer.Deserialize<List<Project>>(projectsEl.GetRawText(), backupOptions);
                if (projects != null) await context.Projects.AddRangeAsync(projects);
            }
            if (root.TryGetProperty("Skills", out var skillsEl))
            {
                var skills = System.Text.Json.JsonSerializer.Deserialize<List<Skill>>(skillsEl.GetRawText(), backupOptions);
                if (skills != null) await context.Skills.AddRangeAsync(skills);
            }
            if (root.TryGetProperty("Messages", out var messagesEl))
            {
                var messages = System.Text.Json.JsonSerializer.Deserialize<List<Message>>(messagesEl.GetRawText(), backupOptions);
                if (messages != null) await context.Messages.AddRangeAsync(messages);
            }

            await context.SaveChangesAsync();
            
            // PostgreSQL'de Identity sequence'larını düzeltmek için (opsiyonel ancak tavsiye edilir, yeni kayıt eklerken hata vermemesi için)
            try
            {
                await context.Database.ExecuteSqlRawAsync(@"
                    SELECT setval('""Experiences_Id_seq""', (SELECT COALESCE(MAX(""Id""), 1) FROM ""Experiences""));
                    SELECT setval('""Projects_Id_seq""', (SELECT COALESCE(MAX(""Id""), 1) FROM ""Projects""));
                    SELECT setval('""Skills_Id_seq""', (SELECT COALESCE(MAX(""Id""), 1) FROM ""Skills""));
                    SELECT setval('""Messages_Id_seq""', (SELECT COALESCE(MAX(""Id""), 1) FROM ""Messages""));
                    SELECT setval('""ProfileInfos_Id_seq""', (SELECT COALESCE(MAX(""Id""), 1) FROM ""ProfileInfos""));
                    SELECT setval('""AppUsers_Id_seq""', (SELECT COALESCE(MAX(""Id""), 1) FROM ""AppUsers""));
                ");
            } catch { /* sequence update may fail if table is empty or names differ, safe to ignore during initial seed */ }
        }
    }
    catch (Exception ex)
    {
        // Migrasyon veya tohumlama sırasında oluşabilecek hataları logla/yazdır
        Console.WriteLine($"[Seeding Error] Veritabanı yapılandırması başarısız oldu: {ex.Message}");
    }
}

app.Run();
