using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.Entities;
using PontoLegal.Service.Interfaces;

namespace PontoLegal.Service;

public class JobPositionService : BaseService, IJobPositionService
{
    private readonly IJobPositionRepository _cargoRepository;

    public JobPositionService(IJobPositionRepository cargoRepository)
    {
        _cargoRepository = cargoRepository;
    }

    public async Task<bool> AddJobPositionAsync(JobPositionModel model)
    {
        return true;
    }
}