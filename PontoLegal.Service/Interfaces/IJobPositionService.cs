using PontoLegal.Service.Entities;

namespace PontoLegal.Service.Interfaces;

public interface IJobPositionService
{
    Task<bool> AddJobPositionAsync(JobPositionModel model);
}