using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;
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
}