using PontoLegal.Domain.ValueObjects;

namespace PontoLegal.Service.DTOs;

public class CompanyDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Cnpj? Cnpj { get; set; }
}