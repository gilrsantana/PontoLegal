using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface IDepartamentoRepository
{
    Task<bool> AddDepartamentoAsync(Departamento departamento);
}
