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
                foreach (var notification in departamento.Notifications)
                {
                    AddNotification(notification);
                }

                return false;
            }

            var departamentoExistente = await GetDepartamentoByNomeAsync(departamento.Nome);

            if (departamentoExistente != null)
            {
                AddNotification("DepartamentoService.Nome", Error.Departamento.NOME_JA_EXISTE);
                return false;

            }
            
            var result = await _departamentoRepository.AddDepartamentoAsync(departamento);
            if (result)
                return true;

            AddNotification("DepartamentoService", Error.Departamento.ERRO_AO_ADICIONAR);
            return false;
           
        }

        public Task<Departamento?> GetDepartamentoByNomeAsync(string departamentoNome)
        {
            return _departamentoRepository.GetDepartamentoByNomeAsync(departamentoNome);
        }

        public Task<Departamento?> GetDepartamentoByIdAsync(Guid departamentoId)
        {
            return _departamentoRepository.GetDepartamentoByIdAsync(departamentoId);
        }


        public async Task<bool> UpdateDepartamentoAsync(Departamento departamento)
        {
            if (!departamento.IsValid)
            {
                foreach (var notification in departamento.Notifications)
                {
                    AddNotification(notification);
                }
                return false;
            }

            var departamentoNomeExistente = await GetDepartamentoByNomeAsync(departamento.Nome);
            var departamentoIdExistente = await GetDepartamentoByIdAsync(departamento.Id);
            if (departamentoIdExistente == null)
            {
                AddNotification("DepartamentoService.Id", Error.Departamento.DEPARTAMENTO_NAO_ENCONTRADO);
                return false;
            }

            if (departamentoNomeExistente != null && 
                departamentoNomeExistente.Id.ToString() != departamentoIdExistente.Id.ToString())
            {
                AddNotification("DepartamentoService.Nome", Error.Departamento.NOME_JA_EXISTE);
                return false;

            }

            var result = await _departamentoRepository.UpdateDepartamentoAsync(departamento);
            if (result)
                return true;

            AddNotification("DepartamentoService", Error.Departamento.ERRO_AO_ATUALIZAR);
            return false;
        }

        public async Task<bool> RemoveDepartamentoAsync(Departamento departamento)
        {
            var departamentoIdExistente = await GetDepartamentoByIdAsync(departamento.Id);
            if (departamentoIdExistente == null)
            {
                AddNotification("DepartamentoService.Id", Error.Departamento.DEPARTAMENTO_NAO_ENCONTRADO);
                return false;
            }
            var result = await _departamentoRepository.RemoveDepartamentoAsync(departamento);
            if (result)
                return true;

            AddNotification("DepartamentoService", Error.Departamento.ERRO_AO_REMOVER);
            return false;
        }
    }
}
