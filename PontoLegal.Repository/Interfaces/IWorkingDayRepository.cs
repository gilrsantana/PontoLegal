using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface IWorkingDayRepository
{
    Task<WorkingDay?> GetWorkingDayByIdAsync(Guid id);
    Task<WorkingDay?> GetWorkingDayByNameAsync(string name);
    Task<ICollection<WorkingDay>?> GetAllWorkingDaysAsync<TResult>(int skip=0, int take=25);
    Task<bool> AddWorkingDayAsync(WorkingDay workingDay);
    Task<bool> UpdateWorkingDayAsync(WorkingDay workingDay);
    Task<bool> RemoveWorkingDayByIdAsync(Guid id);
}