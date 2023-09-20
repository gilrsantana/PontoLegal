using PontoLegal.Domain.ValueObjects;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Entities;

namespace PontoLegal.Service.Interfaces;

public interface ICompanyService
{
    Task<CompanyDTO?> GetCompanyByIdAsync(Guid id);
    Task<CompanyDTO?> GetCompanyByNameAsync(string modelName);
    Task<CompanyDTO?> GetCompanyByCnpjAsync(Cnpj cnpj);
    Task<bool> AddCompanyAsync(CompanyModel model);
    Task<bool> UpdateCompanyAsync(Guid id, CompanyModel model);
    Task<bool> RemoveCompanyByIdAsync(Guid id);
}