using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.Interfaces;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service
{
    public class DepartamentoService : BaseService, IDepartamentoService
    {
        private readonly IDepartamentoRepository _departamentoRepository;

        public DepartamentoService(IDepartamentoRepository departamentoRepository)
        {
            _departamentoRepository = departamentoRepository;
        }

        public async Task<bool> AddDepartamentoAsync(Departamento departamento)
        {
            if (!departamento.IsValid)
            {
                _errors.Add(Error.Departamento.NOME_INVALIDO);
            }
            return false;
        }


    }
}
