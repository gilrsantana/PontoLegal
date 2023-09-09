using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.Interfaces;

namespace PontoLegal.Service;

public class CargoService : BaseService, ICargoService
{
    private readonly ICargoRepository _cargoRepository;

    public CargoService(ICargoRepository cargoRepository)
    {
        _cargoRepository = cargoRepository;
    }

    public async Task<bool> AddCargoAsync(Cargo cargo)
    {
        return true;
    }
}