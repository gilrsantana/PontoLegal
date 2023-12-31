using PontoLegal.Service.DTOs;
using PontoLegal.Service.Models;

namespace PontoLegal.Service.Interfaces;

public interface IWorkingDayService : IBaseService
{
    Task<WorkingDayDTO?> GetWorkingDayByIdAsync(Guid id);
    Task<WorkingDayDTO?> GetWorkingDayByNameAsync(string name);
    Task<ICollection<WorkingDayDTO>> GetAllWorkingDaysAsync(int skip = 0, int take = 25);
    Task<bool> AddWorkingDayAsync(WorkingDayModel model);
    Task<bool> UpdateWorkingDayAsync(Guid id, WorkingDayModel model);
    Task<bool> RemoveWorkingDayByIdAsync(Guid id);
}