using Portfolio.Business.DTOs;

namespace Portfolio.Business.Abstract;

public interface IExperienceService
{
    Task<List<ExperienceDto>> GetAllExperiencesAsync();
    Task AddExperienceAsync(CreateExperienceDto createExperienceDto);
    Task UpdateExperienceAsync(UpdateExperienceDto updateExperienceDto);
    Task DeleteExperienceAsync(int id);
}