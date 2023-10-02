using Common.Model;
using Microsoft.AspNetCore.Mvc;
using PontoLegal.API.Notifications;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Interfaces;
using PontoLegal.Service.Models;
using PontoLegal.Shared.Messages;

namespace PontoLegal.API.Controllers
{
    [Route("PontoLegal/v1/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("GetById/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResultViewModelApi<string>(Error.Company.ID_IS_REQUIRED, MessageType.ERROR));

                var company = await _companyService.GetCompanyByIdAsync(id);
                if (company == null)
                    return NotFound(new ResultViewModelApi<string>(Error.Company.NOT_FOUNDED,
                        MessageType.WARNING));

                return Ok(new ResultViewModelApi<CompanyDTO>(company));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
           
        }

        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return BadRequest(new ResultViewModelApi<string>(Error.Company.NAME_IS_REQUIRED,
                        MessageType.ERROR));

                var company = await _companyService.GetCompanyByNameAsync(name);
                if (company == null)
                    return NotFound(new ResultViewModelApi<string>(Error.Company.NOT_FOUNDED,
                        MessageType.WARNING));
                return Ok(new ResultViewModelApi<CompanyDTO>(company));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }

        [HttpGet("GetByCnpj/{cnpj}")]
        public async Task<IActionResult> GetByCnpj(string cnpj)
        {
            try
            {
                if (string.IsNullOrEmpty(cnpj))
                    return BadRequest(new ResultViewModelApi<string>(Error.Company.INVALID_CNPJ,
                        MessageType.ERROR));

                var company = await _companyService.GetCompanyByCnpjAsync(cnpj);
                if (company == null)
                    return NotFound(new ResultViewModelApi<string>(Error.Company.NOT_FOUNDED,
                        MessageType.WARNING));

                return Ok(new ResultViewModelApi<CompanyDTO>(company));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }

        [HttpGet("GetAll/{skip:int}/{take:int}")]
        public async Task<IActionResult> GetAll(int skip=0, int take=25)
        {
            try
            {
                var companies = await _companyService.GetAllCompaniesAsync(skip, take);

                return Ok(new ResultViewModelApi<IEnumerable<CompanyDTO>>(companies));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyModel model)
        {
            try
            {
                if (!model.IsValid)
                    return BadRequest(
                        new ResultViewModelApi<string>(model.Notifications.Select(n => n.Message).ToList(), MessageType.ERROR));

                var result = await _companyService.AddCompanyAsync(model);

                return StatusCode(201, new ResultViewModelApi<bool>(result));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCompany(Guid id, CompanyModel model)
        {
            try
            {
                if (!model.IsValid)
                    return BadRequest(
                        new ResultViewModelApi<string>(model.Notifications.Select(n => n.Message).ToList(),
                            MessageType.ERROR));

                var result = await _companyService.UpdateCompanyAsync(id, model);

                return Ok(new ResultViewModelApi<bool>(result));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> RemoveCompany(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResultViewModelApi<string>(Error.Company.ID_IS_REQUIRED, MessageType.ERROR));

                var result = await _companyService.RemoveCompanyByIdAsync(id);

                return Ok(new ResultViewModelApi<bool>(result));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }
    }
}
