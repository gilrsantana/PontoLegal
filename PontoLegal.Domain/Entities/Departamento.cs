namespace PontoLegal.Domain.Entities;
public class Departamento : BaseEntity
{
    public string Nome { get; private set; }
    public List<string> Errors { get; set; } = new List<string>();

    public Departamento(string nome)
    {
        Nome = nome;
    }

    public bool IsValid()
    {
        return false;
    }
}
