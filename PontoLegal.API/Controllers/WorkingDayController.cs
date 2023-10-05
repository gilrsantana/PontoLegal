using Common.Model;
using Microsoft.AspNetCore.Mvc;
using PontoLegal.API.Notifications;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Interfaces;
using PontoLegal.Service.Models;
using PontoLegal.Shared.Messages;
using System.Net;

namespace PontoLegal.API.Controllers;

[Route("PontoLegal/api/[controller]")]
[ApiController]
public class WorkingDayController : ControllerBase
{
    private readonly IWorkingDayService _workingDayService;

    public WorkingDayController(IWorkingDayService workingDayService)
    {
        _workingDayService = workingDayService;
    }

    [HttpGet("GetById{id:guid}")]
    [ProducesResponseType(typeof(ResultViewModelApi<WorkingDayDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var workingDay = await _workingDayService.GetWorkingDayByIdAsync(id);

            if (_workingDayService.GetNotifications().Any())
                return BadRequest(new ResultViewModelApi<string>(_workingDayService.GetNotifications(),
                    MessageType.ERROR));

            if (workingDay == null)
                return NotFound(new ResultViewModelApi<string>(Error.WorkingDay.NOT_FOUNDED, MessageType.WARNING));

            return Ok(new ResultViewModelApi<WorkingDayDTO>(workingDay));
        }
        catch (Exception ex)
        {
            return BadRequest(new ResultViewModelApi<string>(ex.Message, MessageType.ERROR));
        }
    }

    [HttpGet("GetByName/{name}")]
    [ProducesResponseType(typeof(ResultViewModelApi<WorkingDayDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByName(string name)
    {
        try
        {
            var workingDay = await _workingDayService.GetWorkingDayByNameAsync(name);

            if (_workingDayService.GetNotifications().Any())
                return BadRequest(new ResultViewModelApi<string>(_workingDayService.GetNotifications(),
                    MessageType.ERROR));

            if (workingDay == null)
                return NotFound(new ResultViewModelApi<string>(Error.WorkingDay.NOT_FOUNDED, MessageType.WARNING));

            return Ok(new ResultViewModelApi<WorkingDayDTO>(workingDay));
        }
        catch (Exception ex)
        {
            return BadRequest(new ResultViewModelApi<string>(ex.Message, MessageType.ERROR));
        }
    }

    [HttpGet("GetAll/{skip:int}/{take:int}")]
    [ProducesResponseType(typeof(ResultViewModelApi<IEnumerable<WorkingDayDTO>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAll(int skip=0, int take=25)
    {
        try
        {
            var workingDays = await _workingDayService.GetAllWorkingDaysAsync(skip, take);

            if (_workingDayService.GetNotifications().Any())
                return BadRequest(new ResultViewModelApi<string>(_workingDayService.GetNotifications(),
                    MessageType.ERROR));

            return Ok(new ResultViewModelApi<IEnumerable<WorkingDayDTO>>(workingDays));
        }
        catch (Exception ex)
        {
            return BadRequest(new ResultViewModelApi<string>(ex.Message, MessageType.ERROR));
        }
    }

    [HttpPost("Add")]
    [ProducesResponseType(typeof(ResultViewModelApi<bool>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddWorkingDay(WorkingDayModel model)
    {
        try
        {
            var result = await _workingDayService.AddWorkingDayAsync(model);
                
            if (result)
                return StatusCode(201, new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_workingDayService.GetNotifications(),
                MessageType.ERROR));
        }
        catch (Exception ex)
        {
            return BadRequest(new ResultViewModelApi<string>(ex.Message, MessageType.ERROR));
        }
    }

    [HttpPut("Update/{id:guid}")]
    [ProducesResponseType(typeof(ResultViewModelApi<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateWorkingDay(Guid id, WorkingDayModel model)
    {
        try
        {
            var result = await _workingDayService.UpdateWorkingDayAsync(id, model);

            if (result)
                return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_workingDayService.GetNotifications(),
                MessageType.ERROR));
        }
        catch (Exception ex)
        {
            return BadRequest(new ResultViewModelApi<string>(ex.Message, MessageType.ERROR));
        }
    }

    [HttpDelete("Delete/{id:guid}")]
    [ProducesResponseType(typeof(ResultViewModelApi<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteWorkingDay(Guid id)
    {
        try
        {
            var result = await _workingDayService.RemoveWorkingDayByIdAsync(id);

            if (result)
                return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_workingDayService.GetNotifications(),
                    MessageType.ERROR));
        }
        catch (Exception ex)
        {
            return BadRequest(new ResultViewModelApi<string>(ex.Message, MessageType.ERROR));
        }
    }

}