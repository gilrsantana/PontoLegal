using Common.Model;
using Microsoft.AspNetCore.Mvc;
using PontoLegal.API.Notifications;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Interfaces;
using System.Net;
using PontoLegal.Service.Models;
using PontoLegal.Shared.Messages;

namespace PontoLegal.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResultViewModelApi<EmployeeDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetEmployeeByIdAsync(Guid id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (_employeeService.GetNotifications().Any())
                return BadRequest(new ResultViewModelApi<string>(_employeeService.GetNotifications(),
                    MessageType.ERROR));
            if (employee == null)
                return NotFound(new ResultViewModelApi<string>(Error.Employee.NOT_FOUNDED,
                    MessageType.WARNING));

            return Ok(new ResultViewModelApi<EmployeeDTO>(employee));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }

    [HttpGet("GetByPis/{pisNumber}")]
    [ProducesResponseType(typeof(ResultViewModelApi<EmployeeDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetEmployeeByPisAsync(string pisNumber)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByPisAsync(pisNumber);

            if (_employeeService.GetNotifications().Any())
                return BadRequest(new ResultViewModelApi<string>(_employeeService.GetNotifications(),
                    MessageType.ERROR));
            if (employee == null)
                return NotFound(new ResultViewModelApi<string>(Error.Employee.NOT_FOUNDED,
                    MessageType.WARNING));

            return Ok(new ResultViewModelApi<EmployeeDTO>(employee));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }

    [HttpPost("Add")]
    [ProducesResponseType(typeof(ResultViewModelApi<bool>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddEmployeeAsync(EmployeeModel model)
    {
        try
        {
            var result = await _employeeService.AddEmployeeAsync(model);

            if (result)
                return StatusCode(201, new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));
  
            return BadRequest(new ResultViewModelApi<string>(_employeeService.GetNotifications(),
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
    public async Task<IActionResult> UpdateEmployeeAsync(Guid id, EmployeeModel model)
    {
        try
        {
            var result = await _employeeService.UpdateEmployeeAsync(id, model);

            if (result)
                return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_employeeService.GetNotifications(),
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
    public async Task<IActionResult> RemoveEmployeeByIdAsync(Guid id)
    {
        try
        {
            var result = await _employeeService.RemoveEmployeeByIdAsync(id);

            if (result)
                return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_employeeService.GetNotifications(),
                MessageType.ERROR));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }
}