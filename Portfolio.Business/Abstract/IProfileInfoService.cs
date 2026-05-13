using Portfolio.Business.DTOs;

namespace Portfolio.Business.Abstract;

public interface IProfileInfoService
{
    // List yerine direkt tek bir nesne dönmesi daha mantıklı (veya listenin ilk elemanını alacağız)
    Task<ProfileInfoDto> GetProfileInfoAsync();
    Task AddProfileInfoAsync(CreateProfileInfoDto createProfileInfoDto);

    // Güncelleme metodunu ekliyoruz
    Task UpdateProfileInfoAsync(ProfileInfoDto updateProfileInfoDto);
}