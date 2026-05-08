using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;
using Portfolio.DataAccess.Contexts;
using Portfolio.Domain.Entities;

namespace Portfolio.Business.Concrete;

public class ProfileInfoManager : IProfileInfoService
{
    private readonly PortfolioDbContext _context;
    private readonly IMapper _mapper;

    public ProfileInfoManager(PortfolioDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProfileInfoDto>> GetProfileInfoAsync()
    {
        var profileInfo = await _context.ProfileInfos.ToListAsync();
        return _mapper.Map<List<ProfileInfoDto>>(profileInfo);
    }

    public async Task AddProfileInfoAsync(CreateProfileInfoDto createProfileInfoDto)
    {
        var profileInfo = _mapper.Map<ProfileInfo>(createProfileInfoDto);
        await _context.ProfileInfos.AddAsync(profileInfo);
        await _context.SaveChangesAsync();
    }
}