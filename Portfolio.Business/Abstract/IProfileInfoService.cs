using Portfolio.Business.DTOs;

namespace Portfolio.Business.Abstract;

public interface IProfileInfoService
{
    Task<List<ProfileInfoDto>> GetProfileInfoAsync();
    Task AddProfileInfoAsync(CreateProfileInfoDto createProfileInfoDto);
}