using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Models;
using PontoLegal.Service.Interfaces;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service;

public class TimeClockService : BaseService, ITimeClockService
{
    private readonly ITimeClockRepository _timeClockRepository;

    public TimeClockService(ITimeClockRepository timeClockRepository)
    {
        _timeClockRepository = timeClockRepository;
    }

    public async Task<bool> AddTimeClockAsync(TimeClockModel model)
    {
        if (!model.IsValid)
        {
            AddNotifications(model.Notifications);
            return false;
        }
        
        var registers = await GetTimeClocksByEmployeeIdAndDateAsync(model.EmployeeId, model.RegisterTime.Date);
        
        if (registers.Any(x => x.RegisterType == model.RegisterType))
        {
            AddNotification("TimeClockService.RegisterTime", Error.TimeClock.INVALID_REGISTER_TIME);
            return false;
        }

        return false;
    }

    private async Task<ICollection<TimeClockDTO>> GetTimeClocksByEmployeeIdAndDateAsync(Guid id, DateTime date)
    {
        if (id == Guid.Empty)
        {
            AddNotification("TimeClockService.EmployeeId", Error.TimeClock.INVALID_EMPLOYEE_ID);
            return new List<TimeClockDTO>();
        }
        
        var registers = await _timeClockRepository.GetTimeClocksByEmployeeIdAndDateAsync(id, date);
        if (!registers.Any())
        {
            AddNotification("TimeClockService.RegisterTime", Error.TimeClock.INVALID_REGISTER_TIME);
            return new List<TimeClockDTO>();
        }

        return registers
            .Select(x => 
                new TimeClockDTO
                {
                    Id = x.Id,
                    RegisterTime = x.RegisterTime,
                    EmployeeId = x.EmployeeId,
                    RegisterType = x.RegisterType,
                    ClockTimeStatus = x.ClockTimeStatus
                })
            .ToList();
    }
}

