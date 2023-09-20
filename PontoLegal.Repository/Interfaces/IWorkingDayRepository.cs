using PontoLegal.Domain.Entities;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Repository.Interfaces;

public interface IWorkingDayRepository
{
    Task<WorkingDay?> GetWorkingDayByNameAsync(string name);
    Task<WorkingDay?> GetWorkingDayByIdAsync(Guid id);
    Task<ICollection<WorkingDay>?> GetAllWorkingDaysAsync<TResult>(int skip=0, int take=25);
    Task<bool> AddWorkingDayAsync(WorkingDay workingDay);
    Task<bool> RemoveWorkingDayByIdAsync(Guid id);
}