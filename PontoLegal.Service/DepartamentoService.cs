using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.Interfaces;

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
            if (departamento.IsValid) 
                return await _departamentoRepository.AddDepartamentoAsync(departamento);
            
            foreach (var notification in departamento.Notifications)
            {
                AddNotification(notification);
            }

            return false;
        }


    }
}
