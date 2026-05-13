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

    public async Task<ProfileInfoDto> GetProfileInfoAsync()
    {
        var profile = await _context.ProfileInfos.FirstOrDefaultAsync();
        return _mapper.Map<ProfileInfoDto>(profile);
    }

    public async Task UpdateProfileInfoAsync(ProfileInfoDto updateProfileInfoDto)
    {
        var profile = _mapper.Map<ProfileInfo>(updateProfileInfoDto);
        _context.ProfileInfos.Update(profile);
        await _context.SaveChangesAsync();
    }

    public async Task AddProfileInfoAsync(CreateProfileInfoDto createProfileInfoDto)
    {
        var profileInfo = _mapper.Map<ProfileInfo>(createProfileInfoDto);
        await _context.ProfileInfos.AddAsync(profileInfo);
        await _context.SaveChangesAsync();
    }
}