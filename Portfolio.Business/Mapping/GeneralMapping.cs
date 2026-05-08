using AutoMapper;
using Portfolio.Business.DTOs;
using Portfolio.Domain.Entities;

namespace Portfolio.Business.Mapping;

// Sınıfımız Profile'dan miras almalıdır
public class GeneralMapping : Profile
{
    public GeneralMapping()
    {
        // ReverseMap() sayesinde işlem iki yönlü çalışır (Entity -> DTO ve DTO -> Entity)
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<Project, CreateProjectDto>().ReverseMap();
        CreateMap<Project, UpdateProjectDto>().ReverseMap();

        CreateMap<Experience, ExperienceDto>().ReverseMap();
        CreateMap<Experience, CreateExperienceDto>().ReverseMap();
        CreateMap<Experience, UpdateExperienceDto>().ReverseMap();

        CreateMap<Skill, SkillDto>().ReverseMap();
        CreateMap<Skill, CreateSkillDto>().ReverseMap();
        CreateMap<Skill, UpdateSkillDto>().ReverseMap();

        CreateMap<ProfileInfo, ProfileInfoDto>().ReverseMap();
        CreateMap<ProfileInfo, CreateProfileInfoDto>().ReverseMap();
        CreateMap<ProfileInfo, UpdateProfileInfoDto>().ReverseMap();


    }
}