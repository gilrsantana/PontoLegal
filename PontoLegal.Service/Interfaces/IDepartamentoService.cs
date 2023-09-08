using PontoLegal.Domain.Entities;

namespace PontoLegal.Service.Interfaces
{
    public interface IDepartamentoService
    {
        Task<bool> AddDepartamentoAsync(Departamento departamento);
        bool IsValid();
    }
}
