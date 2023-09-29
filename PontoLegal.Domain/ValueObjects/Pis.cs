using System.Text.RegularExpressions;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Domain.ValueObjects;

public class Pis
{
    public string Number { get; }
    private readonly ICollection<string> _errors = new List<string>();
    public IEnumerable<string> GetErrors() => _errors.ToList();
    public bool IsValid => _errors.Count == 0;
    
    public Pis(string number)
    {
        Number = number;
        Validate(number);
    }
    
    private void Validate(string number)
    {
        var pattern = @"^\d{11}$";
        if (!Regex.IsMatch(number, pattern))
        {
            _errors.Add(Error.Pis.INVALID_PIS_FORMAT);
            return;
        }
        
        var cleanedPis = new string(Number.ToCharArray(), 0, 11);

        int[] weights = { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var sum = 0;

        for (int i = 0; i < 10; i++)
        {
            int digit = cleanedPis[i] - '0';
            sum += digit * weights[i];
        }

        var remainder = sum % 11;
        var expectedCheckDigit = 11 - remainder;
        
        var actualCheckDigit = int.Parse(cleanedPis.Substring(10, 1));

        if (expectedCheckDigit != actualCheckDigit)
        {
            _errors.Add(Error.Pis.INVALID_PIS_DIGITS);
        }
    }
    
    public override string ToString()
    {
        return $"{Number.Substring(0, 3)}." +
               $"{Number.Substring(3, 5)}." +
               $"{Number.Substring(8, 2)}-" +
               $"{Number.Substring(10, 1)}";
    }
}