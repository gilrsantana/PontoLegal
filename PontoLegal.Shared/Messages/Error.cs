namespace PontoLegal.Shared.Messages;

public static class Error
{
    public static class Departamento
    {
        public const string ERRO_AO_ADICIONAR = "Erro ao adicionar o departamento";
        public const string ERRO_AO_REMOVER = "Erro ao remover o departamento";
        public const string ERRO_AO_ATUALIZAR = "Erro ao atualizar o departamento";
        public const string DEPARTAMENTO_NAO_ENCONTRADO = "Departamento não encontrado";
        public const string NOME_JA_EXISTE = "Nome de departamento já existe";
        public const string NOME_INVALIDO = "Nome de departamento não pode ser nulo e precisa ter entre 3 e 30 caracteres.";
    }

    public static class Cargo
    {
        public static string NOME_INVALIDO = "Nome de cargo não pode ser nulo e precisa ter entre 3 e 30 caracteres.";
    }
}
