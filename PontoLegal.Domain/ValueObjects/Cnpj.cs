using System.Text.RegularExpressions;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Domain.ValueObjects;

public class Cnpj
{
    public string Number { get; }
    private readonly ICollection<string> _errors = new List<string>();
    public IEnumerable<string> GetErrors() => _errors.ToList();
    public bool IsValid => _errors.Count == 0;

    public Cnpj(string number)
    {
        Number = number;
        Validate(number);
    }

    private void Validate(string number)
    {
        var pattern = @"^\d{14}$";
        if (!Regex.IsMatch(number, pattern))
        {
            _errors.Add(Error.Cnpj.INVALID_CNPJ_FORMAT);
            return;
        }
        
        var cleanedCnpj = new string(Number.ToCharArray(), 0, 14);
        // Calculate the first check digit
        var sum = 0;
        var weight = 5;
        for (int i = 0; i < 12; i++)
        {
            sum += int.Parse(cleanedCnpj[i].ToString()) * weight;
            weight = (weight == 2) ? 9 : weight - 1;
        }
        var remainder = sum % 11;
        var firstCheckDigit = (remainder < 2) ? 0 : 11 - remainder;
        
        // Calculate the second check digit
        sum = 0;
        weight = 6;
        for (int i = 0; i < 13; i++)
        {
            sum += int.Parse(cleanedCnpj[i].ToString()) * weight;
            weight = (weight == 2) ? 9 : weight - 1;
        }
        remainder = sum % 11;
        var secondCheckDigit = (remainder < 2) ? 0 : 11 - remainder;
        
        // Check if the calculated check digits match the provided check digits
        if (int.Parse(cleanedCnpj[12].ToString()) == firstCheckDigit &&
            int.Parse(cleanedCnpj[13].ToString()) == secondCheckDigit)
            return;
        
        _errors.Add(Error.Cnpj.INVALID_CNPJ_DIGITS);
    }

    public override string ToString()
    {
        return $"{Number.Substring(0, 2)}." +
               $"{Number.Substring(2, 3)}." +
               $"{Number.Substring(5, 3)}/" +
               $"{Number.Substring(8, 4)}-" +
               $"{Number.Substring(12, 2)}";
    }
    
    
}