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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
