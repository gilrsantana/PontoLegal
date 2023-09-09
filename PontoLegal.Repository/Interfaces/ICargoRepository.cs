using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface ICargoRepository
{
    Task<bool> AddCargoAsync(Cargo cargo);
}