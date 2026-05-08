using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;
using Portfolio.DataAccess.Contexts;
using Portfolio.Domain.Entities;

namespace Portfolio.Business.Concrete;

public class SkillManager : ISkillService
{
    private readonly PortfolioDbContext _context;
    private readonly IMapper _mapper;

    public SkillManager(PortfolioDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SkillDto>> GetAllSkillsAsync()
    {
        var skills = await _context.Skills.ToListAsync();
        return _mapper.Map<List<SkillDto>>(skills);
    }

    public async Task AddSkillAsync(CreateSkillDto createSkillDto)
    {
        var skill = _mapper.Map<Skill>(createSkillDto);
        await _context.Skills.AddAsync(skill);
        await _context.SaveChangesAsync();
    }
}