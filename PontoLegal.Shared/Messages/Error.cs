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
        public const string DEPARTAMENTO_NAO_ENCONTRADO = "Departamento não encontrado";
        public const string NOME_JA_EXISTE = "Nome de departamento já existe";
        public const string NOME_INVALIDO = "Nome de departamento não pode ser nulo e precisa ter entre 3 e 30 caracteres.";
    }
}
