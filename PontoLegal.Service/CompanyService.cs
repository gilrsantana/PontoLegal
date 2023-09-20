using System.Collections;
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

    public async Task<CompanyDTO?> GetCompanyByIdAsync(Guid id)
    {
        if (!ValidateIdForSearch(id)) return null;
        
        var company = await _companyRepository.GetCompanyByIdAsync(id);
        
        return company == null
            ? null
            : new CompanyDTO { Id = company.Id, Name = company.Name, Cnpj = company.Cnpj };
    }
    
    public async Task<CompanyDTO?> GetCompanyByNameAsync(string modelName)
    {
        if (!ValidateNameForSearch(modelName)) return null;
        
        var company = await _companyRepository.GetCompanyByNameAsync(modelName);
        if (company == null) return null;
        return new CompanyDTO { Id = company.Id, Name = company.Name, Cnpj = company.Cnpj };
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
    
    public async Task<ICollection<CompanyDTO>> GetAllCompaniesAsync(int skip=0, int take=25)
    {
        if (skip < 0 || take < 1)
        {
            AddNotification("CompanyService", Error.Company.INVALID_PAGINATION);
            return new List<CompanyDTO>();
        }
        
        var result = await _companyRepository.GetAllCompaniesAsync(skip, take);
        if (result != null)
        {
            var companies = new List<CompanyDTO>();
            foreach (var company in result)
            {
                companies.Add(new CompanyDTO { Id = company.Id, Name = company.Name, Cnpj = company.Cnpj });
            }
            return companies;
        }
        return new List<CompanyDTO>();
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
        
        AddNotification("CompanyService", Error.Company.ERROR_ADDING);
        return false;
    }
    
    public async Task<bool> UpdateCompanyAsync(Guid id, CompanyModel model)
    {
        if (!ValidateIdForSearch(id)) return false;
        
        if (!model.IsValid)
        {
            AddNotifications(model.Notifications);
            return false;
        }
        
        var dtoById = await GetCompanyByIdAsync(id);
        var dtoByCnpj = await GetCompanyByCnpjAsync(model.Cnpj);
        var nameExists = await GetCompanyByNameAsync(model.Name);

        if (dtoById == null)
        {
            AddNotification("CompanyService.Id", Error.Company.NOT_FOUNDED);
            return false;
        }
        
        if (dtoByCnpj != null && dtoByCnpj.Id != id)
        {
            AddNotification("CompanyService.Cnpj", Error.Company.ALREADY_EXISTS);
            return false;
        }
        
        if (nameExists != null && nameExists.Id != id)
        {
            AddNotification("CompanyService.Name", Error.Company.ALREADY_EXISTS);
            return false;
        }
        
        var company = new Company(model.Name, model.Cnpj);
        var result = await _companyRepository.UpdateCompanyAsync(id, company);

        if (result) return true;
        
        AddNotification("CompanyService", Error.Company.ERROR_UPDATING);
        return false;
    }
    
    public async Task<bool> RemoveCompanyByIdAsync(Guid id)
    {
        if (!ValidateIdForSearch(id)) return false;
        
        var companyDto = await GetCompanyByIdAsync(id);
        
        if (companyDto == null) return false;
        
        var result = await _companyRepository.RemoveCompanyByIdAsync(id);

        return result;
    }
    
    private bool ValidateNameForSearch(string name)
    {
        if (!string.IsNullOrWhiteSpace(name)) return true;
        AddNotification("Company.Name", Error.Company.NAME_IS_REQUIRED);
        return false;
    }

    private bool ValidateIdForSearch(Guid id)
    {
        if (id != Guid.Empty) return true;
        AddNotification("Company.Id", Error.Company.ID_IS_REQUIRED);
        return false;
    }
}