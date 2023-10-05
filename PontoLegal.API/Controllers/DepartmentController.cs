using Common.Model;
using Microsoft.AspNetCore.Mvc;
using PontoLegal.API.Notifications;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Interfaces;
using PontoLegal.Service.Models;
using PontoLegal.Shared.Messages;
using System.Net;

namespace PontoLegal.API.Controllers;

[Route("PontoLegal/v1/[controller]")]
[ApiController]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet("GetById/{id:guid}")]
    [ProducesResponseType(typeof(ResultViewModelApi<DepartmentDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);

            if (_departmentService.GetNotifications().Any())
                return BadRequest(new ResultViewModelApi<string>(_departmentService.GetNotifications(),
                    MessageType.ERROR));

            if (department == null)
                return NotFound(new ResultViewModelApi<string>(Error.Department.DEPARTMENT_NOT_FOUNDED, MessageType.WARNING));
                
            return Ok(new ResultViewModelApi<DepartmentDTO>(department));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
           
    }

    [HttpGet("GetByName/{name}")]
    [ProducesResponseType(typeof(ResultViewModelApi<DepartmentDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByName(string name)
    {
        try
        {
            var department = await _departmentService.GetDepartmentByNameAsync(name);

            if (_departmentService.GetNotifications().Any())
                return BadRequest(new ResultViewModelApi<string>(_departmentService.GetNotifications(),
                    MessageType.ERROR));

            if (department == null)
                return NotFound(new ResultViewModelApi<string>(Error.Department.DEPARTMENT_NOT_FOUNDED, MessageType.WARNING));
                

            return Ok(new ResultViewModelApi<DepartmentDTO>(department));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }

    [HttpGet("GetAll/{skip:int}/{take:int}")]
    [ProducesResponseType(typeof(ResultViewModelApi<IEnumerable<DepartmentDTO>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAll(int skip=0, int take=25)
    {
        try
        { 
            var departments = await _departmentService.GetAllDepartmentsAsync(skip, take);

            if (_departmentService.GetNotifications().Any())
                return BadRequest(new ResultViewModelApi<string>(_departmentService.GetNotifications(),
                    MessageType.ERROR));

            return Ok(new ResultViewModelApi<IEnumerable<DepartmentDTO>?>(departments));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }

    [HttpPost("Add")]
    [ProducesResponseType(typeof(ResultViewModelApi<bool>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddDepartment(DepartmentModel model)
    {
        try
        {
            var result = await _departmentService.AddDepartmentAsync(model);

            if (result)
                return StatusCode(201,new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_departmentService.GetNotifications(),
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
    public async Task<IActionResult> UpdateDepartment(Guid id, DepartmentModel model)
    {
        try
        {
            var result = await _departmentService.UpdateDepartmentAsync(id, model);

            if (result)
                return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_departmentService.GetNotifications(),
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
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        try
        {
            var result = await _departmentService.RemoveDepartmentByIdAsync(id);

            if (result)
                return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_departmentService.GetNotifications(),
                MessageType.ERROR));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }
}