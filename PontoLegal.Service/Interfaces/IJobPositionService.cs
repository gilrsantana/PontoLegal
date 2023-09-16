using PontoLegal.Service.DTOs;
using PontoLegal.Service.Entities;

namespace PontoLegal.Service.Interfaces;

public interface IJobPositionService
{
    Task<bool> AddJobPositionAsync(JobPositionModel model);
    Task<JobPositionDTO?> GetJobPositionByNameAsync(string name);
    Task<JobPositionDTO?> GetJobPositionByNameIncludeDepartmentAsync(string name);
}