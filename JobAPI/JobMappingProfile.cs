using AutoMapper;
using JobAPI.Models;
using SharedLibrary.DTOs;

namespace JobAPI
{
    public class JobMappingProfile : Profile
    {
        public JobMappingProfile()
        {
            CreateMap<Job, JobReadDto>();
            CreateMap<JobUpdateDto, Job>();

            CreateMap<JobCreateDto, Job>();
        }
    }
}
