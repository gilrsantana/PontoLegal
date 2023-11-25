using Common.Model;
using Microsoft.AspNetCore.Mvc;
using PontoLegal.API.Notifications;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Interfaces;
using System.Net;
using PontoLegal.Service.Models;
using PontoLegal.Domain.Enums;

namespace PontoLegal.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TimeClockController : ControllerBase
{
    private readonly ITimeClockService _timeClockService;

    public TimeClockController(ITimeClockService timeClockService)
    {
        _timeClockService = timeClockService;
    }

    [HttpGet("{id:guid}/{date:datetime}")]
    [ProducesResponseType(typeof(ResultViewModelApi<IEnumerable<TimeClockDTO>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetTimeClocksByEmployeeIdAndDateAsync(Guid id, DateTime date)
    {
        try
        {
            var registers = await _timeClockService.GetTimeClocksByEmployeeIdAndDateAsync(id, date);
            if (_timeClockService.GetNotifications().Any())
                return BadRequest(new ResultViewModelApi<string>(_timeClockService.GetNotifications(),
                    MessageType.ERROR));
  

            return Ok(new ResultViewModelApi<IEnumerable<TimeClockDTO>>(registers));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }

    [HttpPost("Add")]
    [ProducesResponseType(typeof(ResultViewModelApi<bool>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddTimeClockAsync(TimeClockModel timeClock)
    {
        try
        {
            var result = await _timeClockService.AddTimeClockAsync(timeClock);

            if (result)
                return StatusCode(201, new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_timeClockService.GetNotifications(),
                MessageType.ERROR));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }

    [HttpPut("Update/{timeClockId:guid}")]
    [ProducesResponseType(typeof(ResultViewModelApi<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateTimeClockAsync(Guid timeClockId, ClockTimeStatus status)
    {
        try
        {
            var result = await _timeClockService.UpdateTimeClockStatus(timeClockId, status);

            if (result) 
                return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_timeClockService.GetNotifications(),
                MessageType.ERROR));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }
}