using SharedLibrary.DTOs;

namespace JobAPI.Services
{
    public interface IJobService
    {
        Task<JobReadDto> CreateStudentAsync(JobCreateDto account);
        Task<JobReadDto?> GetStudentByIdAsync(int id);
        Task<IEnumerable<JobReadDto>> GetAllStudentsAsync();
        Task<JobReadDto?> UpdateStudentAsync(int id, JobUpdateDto account);
        Task<bool> DeleteStudentAsync(int id);
        IQueryable<JobReadDto> GetAllAsQueryable();
    }
}
