using PontoLegal.Domain.Entities;
using PontoLegal.Domain.ValueObjects;

namespace PontoLegal.Repository.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetCompanyByIdAsync(Guid id);
    Task<Company?> GetCompanyByNameAsync(string name);
    Task<Company?> GetCompanyByCnpjAsync(Cnpj cnpj); 
    Task<ICollection<Company>?> GetAllCompaniesAsync(int skip=0, int take=25);
    Task<bool> AddCompanyAsync(Company company);
    Task<bool> UpdateCompanyAsync(Company company);
    Task<bool> RemoveCompanyByIdAsync(Guid id);
}