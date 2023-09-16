using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface IJobPositionRepository
{
    Task<bool> AddJobPositionAsync(JobPosition jobPosition);
    Task<JobPosition?> GetJobPositionByNameAsync(string modelName);
    Task<JobPosition?> GetJobPositionByNameIncludeDepartmentAsync(string modelName);
    Task<JobPosition?> GetJobPositionByIdAsync(Guid id);
    Task<JobPosition?> GetJobPositionByIdIncludeDepartmentAsync(Guid id);
}