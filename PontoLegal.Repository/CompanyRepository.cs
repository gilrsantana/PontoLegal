using Microsoft.EntityFrameworkCore;
using PontoLegal.Data;
using PontoLegal.Domain.Entities;
using PontoLegal.Domain.ValueObjects;
using PontoLegal.Repository.Interfaces;

namespace PontoLegal.Repository;

public class CompanyRepository : ICompanyRepository
{
    private readonly PontoLegalContext _context;

    public CompanyRepository(PontoLegalContext context)
    {
        _context = context;
    }

    public async Task<Company?> GetCompanyByIdAsync(Guid id)
    {
        return await _context
            .Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Company?> GetCompanyByNameAsync(string name)
    {
        return await _context
            .Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<Company?> GetCompanyByCnpjAsync(Cnpj cnpj)
    {
        return await _context
            .Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Cnpj == cnpj);
    }

    public async Task<ICollection<Company>?> GetAllCompaniesAsync(int skip = 0, int take = 25)
    {
        return await _context
            .Companies
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<bool> AddCompanyAsync(Company company)
    {
        _context.Companies.Add(company);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateCompanyAsync(Company company)
    {
        _context.Companies.Update(company);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveCompanyByIdAsync(Guid id)
    {
        var company = await GetCompanyByIdAsync(id);
        if (company == null)
            return false;
        _context.Companies.Remove(company);
        return await _context.SaveChangesAsync() > 0;
    }
}