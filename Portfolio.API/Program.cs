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
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PortfolioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Sisteme diyoruz ki: "Biri senden IProjectService isterse, ona ProjectManager ver."
// AutoMapper'a "Profil kurallarýný bu projedeki Assembly içinde ara" diyoruz
// AutoMapper'ý manuel konfigürasyon ile ekliyoruz
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<Portfolio.Business.Mapping.GeneralMapping>();
});
builder.Services.AddScoped<IExperienceService, ExperienceManager>();
builder.Services.AddScoped<IProjectService, ProjectManager>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<ISkillService, SkillManager>();
builder.Services.AddScoped<IProfileInfoService, ProfileInfoManager>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Portfolio API", Version = "v1" });

    // 1. Swagger'a JWT kullanacaðýmýzý tanýmlýyoruz
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Lütfen token'ý þu formatta girin: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // 2. Bu güvenliðin tüm kilitli (Authorize) endpointler için geçerli olacaðýný söylüyoruz
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
// Authentication Ayarlarý
var jwtSettings = builder.Configuration.GetSection("Jwt");
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication(); // Önce kimlik doðrula

app.UseAuthorization();  // Sonra yetkilendir

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
