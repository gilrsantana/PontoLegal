using System.Runtime.CompilerServices;
using PontoLegal.Domain.Entities;
using PontoLegal.Domain.Enums;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Models;
using PontoLegal.Service.Interfaces;
using PontoLegal.Shared.Messages;
using TimeClock = PontoLegal.Domain.Entities.TimeClock;
using Flunt.Notifications;

[assembly: InternalsVisibleTo("PontoLegal.Tests")]
namespace PontoLegal.Service;

public class TimeClockService : BaseService, ITimeClockService
{
    private readonly ITimeClockRepository _timeClockRepository;
    private readonly IEmployeeService _employeeService;
    private readonly IWorkingDayService _workingDayService;
    private readonly ITimeClockNotificationService _timeClockNotificationService;
    public TimeClockService(
        ITimeClockRepository timeClockRepository, 
        IEmployeeService employeeService, 
        IWorkingDayService workingDayService, 
        ITimeClockNotificationService timeClockNotificationService)
    {
        _timeClockRepository = timeClockRepository;
        _employeeService = employeeService;
        _workingDayService = workingDayService;
        _timeClockNotificationService = timeClockNotificationService;
    }

    public async Task<ICollection<TimeClockDTO>> GetTimeClocksByEmployeeIdAndDateAsync(Guid id, DateTime date)
    {
        if (id == Guid.Empty)
        {
            AddNotification("TimeClockService.EmployeeId", Error.TimeClock.INVALID_EMPLOYEE_ID);
            return new List<TimeClockDTO>();
        }

        var registers = await _timeClockRepository.GetTimeClocksByEmployeeIdAndDateAsync(id, date);
        if (!registers.Any())
        {
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

        var timeClock = new TimeClock(model.EmployeeId, model.RegisterType);

        timeClock.TimeClockAddedEventHandler += TimeClock_Added;

        var result = await _timeClockRepository.AddTimeClockAsync(timeClock);

        if (!result)
        {
            AddNotification("TimeClockService", Error.TimeClock.ADD_TIME_CLOCK_ERROR);
            return false;
        }

        return true;
    }

    public async Task TimeClock_Added(object sender, EventArgs e)
    {
        var timeClock = sender as TimeClock;
        if (timeClock == null) return;

        var employee = await _employeeService.GetEmployeeByIdAsync(timeClock.EmployeeId);
        if (employee == null) return;

        var workingDay = await _workingDayService.GetWorkingDayByIdAsync(employee.WorkingDayId);
        if (workingDay == null) return;

        switch (timeClock.RegisterType)
        {
            case RegisterType.START_WORKING_DAY:
                if (timeClock.RegisterTime.TimeOfDay > workingDay.StartWork.AddMinutes(workingDay.MinutesTolerance).ToTimeSpan() ||
                    timeClock.RegisterTime.TimeOfDay < workingDay.StartWork.AddMinutes(-workingDay.MinutesTolerance).ToTimeSpan())
                {
                    timeClock.SetClockTimeStatus(ClockTimeStatus.PENDING);
                    var notification = new TimeClockNotification(timeClock.Id, NotificationStatus.PENDING);
                    var result = await _timeClockNotificationService.AddNotificationAsync(notification);
                }
                break;
            case RegisterType.END_WORKING_DAY:
                if (timeClock.RegisterTime.TimeOfDay > workingDay.EndWork.AddMinutes(workingDay.MinutesTolerance).ToTimeSpan() ||
                    timeClock.RegisterTime.TimeOfDay < workingDay.EndWork.AddMinutes(-workingDay.MinutesTolerance).ToTimeSpan())
                {
                    timeClock.SetClockTimeStatus(ClockTimeStatus.PENDING);
                    var notification = new TimeClockNotification(timeClock.Id, NotificationStatus.PENDING);
                    var result = await _timeClockNotificationService.AddNotificationAsync(notification);
                }
                break;
            case RegisterType.START_BREAK:
                if (timeClock.RegisterTime.TimeOfDay > workingDay.StartBreak.AddMinutes(workingDay.MinutesTolerance).ToTimeSpan() ||
                    timeClock.RegisterTime.TimeOfDay < workingDay.StartBreak.AddMinutes(-workingDay.MinutesTolerance).ToTimeSpan())
                {
                    timeClock.SetClockTimeStatus(ClockTimeStatus.PENDING);
                    var notification = new TimeClockNotification(timeClock.Id, NotificationStatus.PENDING);
                    var result = await _timeClockNotificationService.AddNotificationAsync(notification);
                }
                break;
            case RegisterType.END_BREAK:
                if (timeClock.RegisterTime.TimeOfDay > workingDay.EndBreak.AddMinutes(workingDay.MinutesTolerance).ToTimeSpan() ||
                    timeClock.RegisterTime.TimeOfDay < workingDay.EndBreak.AddMinutes(-workingDay.MinutesTolerance).ToTimeSpan())
                {
                    timeClock.SetClockTimeStatus(ClockTimeStatus.PENDING);
                    var notification = new TimeClockNotification(timeClock.Id, NotificationStatus.PENDING);
                    var result = await _timeClockNotificationService.AddNotificationAsync(notification);
                }
                break;
            default:
                break;
        }
    }
    
    public async Task<bool> UpdateTimeClockStatus(Guid timeClockId, ClockTimeStatus status)
    {
        if (timeClockId == Guid.Empty)
        {
            AddNotification("TimeClockService.TimeClockId", Error.TimeClock.INVALID_TIME_CLOCK_ID);
            return false;
        }

        var timeClock = await _timeClockRepository.GetTimeClockByIdAsync(timeClockId);
        if (timeClock == null)
        {
            AddNotification("TimeClockService.TimeClockId", Error.TimeClock.TIME_CLOCK_NOT_FOUND);
            return false;
        }
        timeClock.SetClockTimeStatus(status);
        var result = await _timeClockRepository.UpdateTimeClockAsync(timeClock);

        if (!result)
        {
            AddNotification("TimeClockService", Error.TimeClock.UPDATE_TIME_CLOCK_ERROR);
            return false;
        }

        return true;
    }
}

