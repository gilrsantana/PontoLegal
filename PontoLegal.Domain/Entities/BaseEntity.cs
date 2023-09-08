namespace PontoLegal.Domain.Entities;
public class BaseEntity
{
    public Guid Id { get; private set; }
    public DateTime DataCriacao { get; private set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
        DataCriacao = DateTime.Now;
    }
}