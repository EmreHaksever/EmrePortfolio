using Portfolio.Business.DTOs;

namespace Portfolio.Business.Abstract;

public interface IExperienceService
{
    Task<List<ExperienceDto>> GetAllExperiencesAsync();
    Task<ExperienceDto> GetExperienceByIdAsync(int id);
    Task AddExperienceAsync(CreateExperienceDto createExperienceDto);
    Task UpdateExperienceAsync(UpdateExperienceDto updateExperienceDto);
    Task DeleteExperienceAsync(int id);
}