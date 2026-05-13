using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;
using Portfolio.DataAccess.Contexts;
using Portfolio.Domain.Entities;

namespace Portfolio.Business.Concrete;

public class ExperienceManager : IExperienceService
{
    private readonly PortfolioDbContext _context;
    private readonly IMapper _mapper; // AutoMapper'ı içeri alıyoruz

    public ExperienceManager(PortfolioDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ExperienceDto>> GetAllExperiencesAsync()
    {
        var experiences = await _context.Experiences.ToListAsync();

        // AutoMapper Büyüsü: Tüm listeyi tek satırda DTO listesine çevirir!
        return _mapper.Map<List<ExperienceDto>>(experiences);
    }

    public async Task<ExperienceDto> GetExperienceByIdAsync(int id)
    {
        // 1. Veritabanından veriyi çekiyoruz (Dal veya Repository ismin neyse ona göre uyarla)
        var experience = await _context.Experiences.FindAsync(id);
        // veya: await _experienceRepository.GetByIdAsync(id);

        if (experience == null)
        {
            return null;
        }
        return _mapper.Map<ExperienceDto>(experience);
    }
    public async Task AddExperienceAsync(CreateExperienceDto createExperienceDto)
    {
        // Gelen DTO nesnesini tek satırda Entity nesnesine çevirir!
        var experience = _mapper.Map<Experience>(createExperienceDto);

        await _context.Experiences.AddAsync(experience);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateExperienceAsync(UpdateExperienceDto updateExperienceDto)
    {
        var experience = _mapper.Map<Experience>(updateExperienceDto);
        _context.Experiences.Update(experience);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExperienceAsync(int id)
    {
        var experience = await _context.Experiences.FindAsync(id);
        if (experience != null)
        {
            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();
        }
    }
}