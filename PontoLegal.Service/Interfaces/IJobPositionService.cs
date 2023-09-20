using PontoLegal.Service.DTOs;
using PontoLegal.Service.Entities;

namespace PontoLegal.Service.Interfaces;

public interface IJobPositionService
{
    Task<JobPositionDTO?> GetJobPositionByIdAsync(Guid id);
    Task<JobPositionDTO?> GetJobPositionByIdIncludeDepartmentAsync(Guid id);
    Task<JobPositionDTO?> GetJobPositionByNameAsync(string name);
    Task<JobPositionDTO?> GetJobPositionByNameIncludeDepartmentAsync(string name);
    Task<ICollection<JobPositionDTO>> GetAllJobPositionsAsync(int skip = 0, int take = 25);
    Task<bool> AddJobPositionAsync(JobPositionModel model);
    Task<bool> UpdateJobPositionAsync(Guid id, JobPositionModel model);
    Task<bool> RemoveJobPositionByIdAsync(Guid id);
}