using PontoLegal.Domain.ValueObjects;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Models;

namespace PontoLegal.Service.Interfaces;

public interface ICompanyService
{
    Task<CompanyDTO?> GetCompanyByIdAsync(Guid id);
    Task<CompanyDTO?> GetCompanyByNameAsync(string modelName);
    Task<CompanyDTO?> GetCompanyByCnpjAsync(string cnpj);
    Task<ICollection<CompanyDTO>> GetAllCompaniesAsync(int skip = 0, int take = 25);
    Task<bool> AddCompanyAsync(CompanyModel model);
    Task<bool> UpdateCompanyAsync(Guid id, CompanyModel model);
    Task<bool> RemoveCompanyByIdAsync(Guid id);
}