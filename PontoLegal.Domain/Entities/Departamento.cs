using Flunt.Validations;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Domain.Entities;
public class Departamento : BaseEntity
{
    public string Nome { get; private set; }

    public Departamento(string nome)
    {
        Nome = nome;
        AddNotifications(new Contract<Departamento>()
            .Requires()
            .IsNotNullOrEmpty(Nome, "Departamento.Nome", Error.Departamento.NOME_INVALIDO)
            .IsGreaterOrEqualsThan(Nome, 3, "Departamento.Nome", Error.Departamento.NOME_INVALIDO)
            .IsLowerOrEqualsThan(Nome, 30, "Departamento.Nome", Error.Departamento.NOME_INVALIDO)
        );
    }
}
