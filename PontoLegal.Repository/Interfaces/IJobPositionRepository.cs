using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface IJobPositionRepository
{
    Task<bool> AddJobPositionAsync(JobPosition jobPosition);
}