using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface IDepartamentoRepository
{
    Task<bool> AddDepartamentoAsync(Departamento departamento);
    Task<bool> UpdateDepartamentoAsync(Departamento departamento);
    Task<Departamento?> GetDepartamentoByNomeAsync(string departamentoNome);
    Task<Departamento?> GetDepartamentoByIdAsync(Guid departamentoId);
}
