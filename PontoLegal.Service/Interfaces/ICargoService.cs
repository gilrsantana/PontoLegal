using PontoLegal.Domain.Entities;

namespace PontoLegal.Service.Interfaces
{
    public interface ICargoService
    {
        Task<bool> AddCargoAsync(Cargo cargo);
    }
}
