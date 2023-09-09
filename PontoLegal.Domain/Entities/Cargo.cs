using Flunt.Validations;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Domain.Entities;

public class Cargo : BaseEntity
{
    public string Nome { get; private set; }
    public Departamento Departamento { get; private set; }

    public Cargo(string nome, Departamento departamento)
    {
        Nome = nome;
        Departamento = departamento;
        AddNotifications(new Contract<Cargo>()
            .Requires()
            .IsGreaterOrEqualsThan(Nome, 3, "Cargo.Nome", Error.Cargo.NOME_INVALIDO)
            .IsLowerOrEqualsThan(Nome, 30, "Cargo.Nome", Error.Cargo.NOME_INVALIDO)
        );
    }
}