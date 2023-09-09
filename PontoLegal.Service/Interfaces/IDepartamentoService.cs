using PontoLegal.Domain.Entities;

namespace PontoLegal.Service.Interfaces
{
    public interface IDepartamentoService
    {
        Task<bool> AddDepartamentoAsync(Departamento departamento); 
        Task<Departamento?> GetDepartamentoByNomeAsync(string departamentoNome);
        Task<Departamento?> GetDepartamentoByIdAsync(Guid departamentoId);
        Task<bool> UpdateDepartamentoAsync(Departamento departamento);
    }
}
