using PontoLegal.Domain.Entities;
using PontoLegal.Domain.ValueObjects;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Entities;
using PontoLegal.Service.Interfaces;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service;

public class CompanyService : BaseService, ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<bool> AddCompanyAsync(CompanyModel model)
    {
        if (!model.IsValid)
        {
            AddNotifications(model.Notifications);
            return false;
        }
        
        var dtoByCnpj = await GetCompanyByCnpjAsync(model.Cnpj);
        var nameExists = await GetCompanyByNameAsync(model.Name);

        if (dtoByCnpj != null )
        {
            AddNotification("CompanyService.Cnpj", Error.Company.ALREADY_EXISTS);
            return false;
        }
        
        if (nameExists != null)
        {
            AddNotification("CompanyService.Name", Error.Company.ALREADY_EXISTS);
            return false;
        }
        
        var company = new Company(model.Name, model.Cnpj);
        var result = await _companyRepository.AddCompanyAsync(company);

        if (result)
            return true;
        
        AddNotification("CompanyService", Error.Company.ADD_ERROR);
        return false;
    }

    public async Task<CompanyDTO?> GetCompanyByNameAsync(string modelName)
    {
        if (!ValidateNameForSearch(modelName)) return null;
        
        var company = await _companyRepository.GetCompanyByNameAsync(modelName);
        if (company == null) return null;
        return new CompanyDTO { Id = company.Id, Name = company.Name, Cnpj = company.Cnpj };
    }

    private bool ValidateNameForSearch(string name)
    {
        if (!string.IsNullOrWhiteSpace(name)) return true;
        AddNotification("Company.Name", Error.Company.NAME_IS_REQUIRED);
        return false;
    }
    public async Task<CompanyDTO?> GetCompanyByCnpjAsync(Cnpj cnpj)
    {
        if (!cnpj.IsValid)
        {
            foreach (var error in cnpj.GetErrors())
            {
                AddNotification("CompanyService.Cnpj", error);
            }
            return null;
        }

        var company = await _companyRepository.GetCompanyByCnpjAsync(cnpj);
        return company == null
            ? null
            : new CompanyDTO { Id = company.Id, Name = company.Name, Cnpj = company.Cnpj };
    }

    public async Task<CompanyDTO?> GetCompanyByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            AddNotification("CompanyService.Id", Error.Company.INVALID_ID);
            return null;
        }
        
        var company = await _companyRepository.GetCompanyByIdAsync(id);
        
        return company == null
            ? null
            : new CompanyDTO { Id = company.Id, Name = company.Name, Cnpj = company.Cnpj };
    }
}