using PontoLegal.Domain.Entities;
using PontoLegal.Domain.ValueObjects;

namespace PontoLegal.Repository.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetCompanyByIdAsync(Guid id);
    Task<Company?> GetCompanyByNameAsync(string name);
    Task<Company?> GetCompanyByCnpjAsync(Cnpj cnpj); 
    Task<bool> AddCompanyAsync(Company company);
    Task<bool> RemoveCompanyByIdAsync(Guid id);
    
}