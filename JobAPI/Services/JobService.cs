using AutoMapper;
using JobAPI.Models;
using JobAPI.Repositories;
using SharedLibrary.DTOs;

namespace JobAPI.Services
{
    public class JobService(IRepository<Job> _repository, IMapper _mapper) : IJobService
    {
        public async Task<JobReadDto> CreateStudentAsync(JobCreateDto account)
        {
            var accountEntity = _mapper.Map<Job>(account);
            var createdAccount = await _repository.CreateAsync(accountEntity);

            return _mapper.Map<JobReadDto>(createdAccount);
        }

        public Task<bool> DeleteStudentAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public async Task<JobReadDto?> GetStudentByIdAsync(int id)
        {
            var account = await _repository.GetByIdAsync(id);
            return account == null ? null : _mapper.Map<JobReadDto>(account);
        }

        public async Task<IEnumerable<JobReadDto>> GetAllStudentsAsync()
        {
            var accounts = await _repository.GetAllAsync();
            return accounts.Select(account => _mapper.Map<JobReadDto>(account)).ToList();
        }

        public async Task<JobReadDto?> UpdateStudentAsync(int id, JobUpdateDto account)
        {
            var accountEntity = _mapper.Map<Job>(account);
            accountEntity.Id = id;
            var updatedAccount = await _repository.UpdateAsync(id, accountEntity);
            return updatedAccount == null ? null : _mapper.Map<JobReadDto>(updatedAccount);
        }
    }
}
