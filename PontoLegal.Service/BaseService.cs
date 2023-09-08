namespace PontoLegal.Service;

public class BaseService
{
    protected readonly IList<string> _errors = new List<string>();
    public bool IsValid { get => _errors.Count == 0; }
}
