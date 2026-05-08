using Portfolio.Business.DTOs;

namespace Portfolio.Business.Abstract;

public interface ISkillService
{
    Task<List<SkillDto>> GetAllSkillsAsync();
    Task AddSkillAsync(CreateSkillDto createSkillDto);
}