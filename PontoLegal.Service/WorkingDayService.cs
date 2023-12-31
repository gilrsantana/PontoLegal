using System.Runtime.CompilerServices;
using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Models;
using PontoLegal.Service.Interfaces;
using PontoLegal.Shared.Messages;

[assembly: InternalsVisibleTo("PontoLegal.Tests")]
namespace PontoLegal.Service;

public class WorkingDayService : BaseService, IWorkingDayService
{
    private readonly IWorkingDayRepository _workingDayRepository;

    public WorkingDayService(IWorkingDayRepository workingDayRepository)
    {
        _workingDayRepository = workingDayRepository;
    }
    
    public async Task<WorkingDayDTO?> GetWorkingDayByIdAsync(Guid id)
    {
        if (!ValidateIdForSearch(id)) return null;

        var workingDay = await _workingDayRepository.GetWorkingDayByIdAsync(id);
        if (workingDay == null) return null;
        
        return new WorkingDayDTO
        {
            Id = workingDay.Id,
            Name = workingDay.Name,
            Type = workingDay.Type,
            StartWork = workingDay.StartWork,
            StartBreak = workingDay.StartBreak,
            EndBreak = workingDay.EndBreak,
            EndWork = workingDay.EndWork,
            MinutesTolerance = workingDay.MinutesTolerance
        };
    }
    
    public async Task<WorkingDayDTO?> GetWorkingDayByNameAsync(string name)
    {
        if (!ValidateNameForSearch(name)) return null;
        
        var workingDay = await _workingDayRepository.GetWorkingDayByNameAsync(name);
        if (workingDay == null) return null;
        
        return new WorkingDayDTO
        {
            Id = workingDay.Id,
            Name = workingDay.Name,
            Type = workingDay.Type,
            StartWork = workingDay.StartWork,
            StartBreak = workingDay.StartBreak,
            EndBreak = workingDay.EndBreak,
            EndWork = workingDay.EndWork,
            MinutesTolerance = workingDay.MinutesTolerance
        };
    }

    public async Task<ICollection<WorkingDayDTO>> GetAllWorkingDaysAsync(int skip=0, int take=25)
    {
        if (skip < 0 || take < 1)
        {
            AddNotification("WorkingDayService", Error.WorkingDay.INVALID_PAGINATION);
            return new List<WorkingDayDTO>();
        }
        
        var workingDays = await _workingDayRepository.GetAllWorkingDaysAsync<WorkingDay>(skip, take);
        if (workingDays != null && workingDays.Any())
        {
            return workingDays.Select(workingDay => new WorkingDayDTO
            {
                Id = workingDay.Id,
                Name = workingDay.Name,
                Type = workingDay.Type,
                StartWork = workingDay.StartWork,
                StartBreak = workingDay.StartBreak,
                EndBreak = workingDay.EndBreak,
                EndWork = workingDay.EndWork,
                MinutesTolerance = workingDay.MinutesTolerance
            }).ToList();
        }
        return new List<WorkingDayDTO>();
    }

    public async Task<bool> AddWorkingDayAsync(WorkingDayModel model)
    {
        if (!model.IsValid)
        {
            AddNotifications(model.Notifications);
            return false;
        }
        
        var existingWorkingDay = await GetWorkingDayByNameAsync(model.Name);
        
        if (existingWorkingDay != null)
        {
            AddNotification("WorkingDay.Name", Error.WorkingDay.NAME_ALREADY_EXISTS);
            return false;
        }
        
        var workingDay = new WorkingDay(
            model.Name, 
            model.Type, 
            model.StartWork, 
            model.StartBreak, 
            model.EndBreak, 
            model.EndWork,
            model.MinutesTolerance);
        
        var result = await _workingDayRepository.AddWorkingDayAsync(workingDay);
        
        if (!result)
        {
            AddNotification("WorkingDay", Error.WorkingDay.ERROR_ADDING);
            return false;
        }

        return true;
    }
    
    public async Task<bool> UpdateWorkingDayAsync(Guid id, WorkingDayModel model)
    {
        if (!ValidateIdForSearch(id)) return false;
        
        if (!model.IsValid)
        {
            AddNotifications(model.Notifications);
            return false;
        }
        
        var existingWorkingDay = await GetWorkingDayByIdAsync(id);
        if (existingWorkingDay == null)
        {
            AddNotification("WorkingDay.Id", Error.WorkingDay.NOT_FOUNDED);
            return false;
        }
        
        var workingDayByName = await GetWorkingDayByNameAsync(model.Name);
        if (workingDayByName != null && workingDayByName.Id != id)
        {
            AddNotification("WorkingDay.Name", Error.WorkingDay.NAME_ALREADY_EXISTS);
            return false;
        }

        var workingDay = await _workingDayRepository.GetWorkingDayByIdAsync(id);

        workingDay!.Update(model.Name, model.Type, model.StartWork, model.StartBreak, model.EndBreak, model.EndWork, model.MinutesTolerance);

        var result = await _workingDayRepository.UpdateWorkingDayAsync(workingDay);
        
        if (!result)
        {
            AddNotification("WorkingDay", Error.WorkingDay.ERROR_UPDATING);
            return false;
        }
        return true;
    }
    
    public async Task<bool> RemoveWorkingDayByIdAsync(Guid id)
    {
        if (!ValidateIdForSearch(id)) return false;
        
        var existingWorkingDay = await _workingDayRepository.GetWorkingDayByIdAsync(id);
        if (existingWorkingDay == null)
        {
            AddNotification("WorkingDay.Id", Error.WorkingDay.NOT_FOUNDED);
            return false;
        }
        
        var result = await _workingDayRepository.RemoveWorkingDayByIdAsync(id);
        if (!result)
        {
            AddNotification("WorkingDay", Error.WorkingDay.ERROR_REMOVING);
            return false;
        }

        return true;
    }
    
    internal bool ValidateIdForSearch(Guid id)
    {
        if (id != Guid.Empty) return true;
        AddNotification("WorkingDay.Id", Error.WorkingDay.ID_IS_REQUIRED);
        return false;
    }
    
    internal bool ValidateNameForSearch(string name)
    {
        if (!string.IsNullOrWhiteSpace(name)) return true;
        AddNotification("WorkingDay.Name", Error.WorkingDay.NAME_IS_REQUIRED);
        return false;
    }
}