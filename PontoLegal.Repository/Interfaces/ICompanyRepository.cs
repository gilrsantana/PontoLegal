using PontoLegal.Domain.Entities;
using PontoLegal.Domain.ValueObjects;

namespace PontoLegal.Repository.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetCompanyByCnpjAsync(Cnpj cnpj); 
    Task<Company?> GetCompanyByNameAsync(string name);
    Task<bool> AddCompanyAsync(Company company);
    Task<Company?> GetCompanyByIdAsync(Guid id);
}