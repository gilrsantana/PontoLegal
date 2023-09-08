using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PontoLegal.Shared.Messages;

public static class Error
{
    public static class Departamento
    {
        public const string NOME_INVALIDO = "Nome não pode ser nulo e precisa ter entre 3 e 30 caracteres.";
    }
}
