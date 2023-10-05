using Common.Model;
using Microsoft.AspNetCore.Mvc;
using PontoLegal.API.Notifications;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Interfaces;
using PontoLegal.Service.Models;
using PontoLegal.Shared.Messages;
using System.Net;

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
        [ProducesResponseType(typeof(ResultViewModelApi<CompanyDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var company = await _companyService.GetCompanyByIdAsync(id);

                if (_companyService.GetNotifications().Any())
                    return BadRequest(new ResultViewModelApi<string>(_companyService.GetNotifications(),
                        MessageType.ERROR));

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
        [ProducesResponseType(typeof(ResultViewModelApi<CompanyDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var company = await _companyService.GetCompanyByNameAsync(name);

                if (_companyService.GetNotifications().Any())
                    return BadRequest(new ResultViewModelApi<string>(_companyService.GetNotifications(),
                        MessageType.ERROR));

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
        [ProducesResponseType(typeof(ResultViewModelApi<CompanyDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetByCnpj(string cnpj)
        {
            try
            {
                var company = await _companyService.GetCompanyByCnpjAsync(cnpj);

                if (_companyService.GetNotifications().Any())
                    return BadRequest(new ResultViewModelApi<string>(_companyService.GetNotifications(),
                        MessageType.ERROR));

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
        [ProducesResponseType(typeof(ResultViewModelApi<IEnumerable<CompanyDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAll(int skip=0, int take=25)
        {
            try
            {
                var companies = await _companyService.GetAllCompaniesAsync(skip, take);

                if (_companyService.GetNotifications().Any())
                    return BadRequest(new ResultViewModelApi<string>(_companyService.GetNotifications(),
                        MessageType.ERROR));

                return Ok(new ResultViewModelApi<IEnumerable<CompanyDTO>>(companies));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }

        [HttpPost("Add")]
        [ProducesResponseType(typeof(ResultViewModelApi<bool>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddCompany(CompanyModel model)
        {
            try
            {
                var result = await _companyService.AddCompanyAsync(model);

                if (result)
                    return StatusCode(201, new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

                return BadRequest(new ResultViewModelApi<string>(_companyService.GetNotifications(),
                    MessageType.ERROR));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }

        [HttpPut("Update/{id:guid}")]
        [ProducesResponseType(typeof(ResultViewModelApi<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCompany(Guid id, CompanyModel model)
        {
            try
            {
                var result = await _companyService.UpdateCompanyAsync(id, model);
                    
                if (result)
                    return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

                return BadRequest(new ResultViewModelApi<string>(_companyService.GetNotifications(),
                    MessageType.ERROR));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }

        [HttpDelete("Delete/{id:guid}")]
        [ProducesResponseType(typeof(ResultViewModelApi<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveCompany(Guid id)
        {
            try
            {
                var result = await _companyService.RemoveCompanyByIdAsync(id);
                    
                if (result)
                    return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

                return BadRequest(new ResultViewModelApi<string>(_companyService.GetNotifications(),
                    MessageType.ERROR));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }
    }
}
