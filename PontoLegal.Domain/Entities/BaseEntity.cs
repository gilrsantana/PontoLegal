namespace PontoLegal.Domain.Entities;
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
}
