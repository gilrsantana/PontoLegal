using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Entities;
using PontoLegal.Service.Interfaces;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service;

public class JobPositionService : BaseService, IJobPositionService
{
    private readonly IJobPositionRepository _jobPositionRepository;

    public JobPositionService(IJobPositionRepository jobPositionRepository)
    {
        _jobPositionRepository = jobPositionRepository;
    }

    public async Task<bool> AddJobPositionAsync(JobPositionModel model)
    {
        if (!model.IsValid)
        {
            AddNotifications(model.Notifications);
            return false;
        }

        var jobPositionByName = await _jobPositionRepository.GetJobPositionByNameIncludeDepartmentAsync(model.Name);
        if (jobPositionByName != null && 
            jobPositionByName.Department.Name == model.Department.Name && 
            jobPositionByName.Name == model.Name)
        {
            AddNotification("JobPosition.Name", Error.JobPosition.NAME_ALREADY_EXISTS);
            return false;
        }

        var department = new Department(model.Name);

        var jobPosition = new JobPosition(model.Name, department.Id, department);

        var result = await _jobPositionRepository.AddJobPositionAsync(jobPosition);

        if (result) return true;

        AddNotification("JobPosition", Error.JobPosition.ERROR_ADDING);
        return false;
    }

    public async Task<JobPositionDTO?> GetJobPositionByNameAsync(string name)
    {
        if (!ValidateNameForSearch(name)) return null;

        var jobPosition = await _jobPositionRepository.GetJobPositionByNameAsync(name);
        if (jobPosition == null) return null;
        
        return new JobPositionDTO
        {
            Id = jobPosition.Id,
            Name = jobPosition.Name
        };

    }

    public async Task<JobPositionDTO?> GetJobPositionByNameIncludeDepartmentAsync(string name)
    {
        if (!ValidateNameForSearch(name)) return null;

        var jobPosition = await _jobPositionRepository.GetJobPositionByNameIncludeDepartmentAsync(name);
        if (jobPosition == null) return null;

        return new JobPositionDTO
        {
            Id = jobPosition.Id,
            Name = jobPosition.Name,
            Department = new DepartmentDTO
            {
                Id = jobPosition.Department.Id,
                Name = jobPosition.Department.Name
            }
        };
    }

    private bool ValidateNameForSearch(string name)
    {
        if (!string.IsNullOrWhiteSpace(name)) return true;
        AddNotification("JobPosition.Name", Error.JobPosition.NAME_IS_REQUIRED);
        return false;

    }
}