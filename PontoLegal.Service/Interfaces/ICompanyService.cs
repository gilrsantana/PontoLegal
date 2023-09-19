using PontoLegal.Domain.ValueObjects;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Entities;

namespace PontoLegal.Service.Interfaces;

public interface ICompanyService
{
    Task<bool> AddCompanyAsync(CompanyModel model);
    Task<CompanyDTO?> GetCompanyByNameAsync(string modelName);
    Task<CompanyDTO?> GetCompanyByCnpjAsync(Cnpj cnpj);
    Task<CompanyDTO?> GetCompanyByIdAsync(Guid id);
}