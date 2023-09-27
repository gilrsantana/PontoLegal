using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Models;
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

    public async Task<JobPositionDTO?> GetJobPositionByIdAsync(Guid id)
    {
        if (!ValidateIdForSearch(id)) return null;

        var jobPosition = await _jobPositionRepository.GetJobPositionByIdAsync(id);
        if (jobPosition == null) return null;
        
        return new JobPositionDTO
        {
            Id = jobPosition.Id,
            Name = jobPosition.Name
        };
    }
    
    public async Task<JobPositionDTO?> GetJobPositionByIdIncludeDepartmentAsync(Guid id)
    {
        if (!ValidateIdForSearch(id)) return null;

        var jobPosition = await _jobPositionRepository.GetJobPositionByIdIncludeDepartmentAsync(id);
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
    
    public async Task<ICollection<JobPositionDTO>> GetAllJobPositionsAsync(int skip = 0, int take = 25)
    {
        if (skip < 0 || take < 1)
        {
            AddNotification("JobPositionService", Error.JobPosition.INVALID_PAGINATION);
            return new List<JobPositionDTO>();
        }

        var result = await _jobPositionRepository.GetAllJobPositionsAsync(skip, take);
        if (result == null) return new List<JobPositionDTO>();

        return result.Select(jobPosition => new JobPositionDTO
        {
            Id = jobPosition.Id,
            Name = jobPosition.Name
        }).ToList();
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

    public async Task<bool> UpdateJobPositionAsync(Guid id, JobPositionModel model)
    {
        if (!model.IsValid)
        {
            AddNotifications(model.Notifications);
            return false;
        }

        if (!ValidateIdForSearch(id)) return false;

        var jobPosition = await GetJobPositionByIdAsync(id);
        if (jobPosition == null)
        {
            AddNotification("JobPosition.Id", Error.JobPosition.NOT_FOUNDED);
            return false;
        }
        var existentJobPosition = await _jobPositionRepository.GetJobPositionByNameIncludeDepartmentAsync(model.Name);
        if (existentJobPosition != null && 
            existentJobPosition.Department.Name == model.Department.Name && 
            existentJobPosition.Name == model.Name)
        {
            AddNotification("JobPosition.Name", Error.JobPosition.NAME_ALREADY_EXISTS);
            return false;
        }
        
        var department = new Department(model.Name);
        var newJobPosition = new JobPosition(model.Name, department.Id, department);
        
        var result = await _jobPositionRepository.UpdateJobPositionAsync(id, newJobPosition);
        if (result) return true;
        AddNotification("JobPosition", Error.JobPosition.ERROR_UPDATING);
        
        return false;
    }
    
    public async Task<bool> RemoveJobPositionByIdAsync(Guid id)
    {
        if (!ValidateIdForSearch(id)) return false;
        
        var jobPosition = await _jobPositionRepository.GetJobPositionByIdAsync(id);
        if (jobPosition == null)
        {
            AddNotification("JobPosition.Id", Error.JobPosition.NOT_FOUNDED);
            return false;
        }
        
        var result = await _jobPositionRepository.RemoveJobPositionAsync(jobPosition);
        if (result) return true;
        
        AddNotification("JobPosition", Error.JobPosition.ERROR_REMOVING);
        return false;
    }
    
    private bool ValidateNameForSearch(string name)
    {
        if (!string.IsNullOrWhiteSpace(name)) return true;
        AddNotification("JobPosition.Name", Error.JobPosition.NAME_IS_REQUIRED);
        return false;
    }
    
    private bool ValidateIdForSearch(Guid id)
    {
        if (id != Guid.Empty) return true;
        AddNotification("JobPosition.Id", Error.JobPosition.ID_IS_REQUIRED);
        return false;
    }
}