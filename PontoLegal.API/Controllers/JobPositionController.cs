using System.Net;
using Common.Model;
using Microsoft.AspNetCore.Mvc;
using PontoLegal.API.Notifications;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Interfaces;
using PontoLegal.Service.Models;
using PontoLegal.Shared.Messages;

namespace PontoLegal.API.Controllers;

[Route("PontoLegal/v1/[controller]")]
[ApiController]
public class JobPositionController : ControllerBase
{
    private readonly IJobPositionService _jobPositionService;

    public JobPositionController(IJobPositionService jobPositionService)
    {
        _jobPositionService = jobPositionService;
    }

    [HttpGet("GetById/{id:guid}")]
    [ProducesResponseType(typeof(ResultViewModelApi<JobPositionDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest(new ResultViewModelApi<string>(Error.JobPosition.ID_IS_REQUIRED, MessageType.ERROR));

            var jobPosition = await _jobPositionService.GetJobPositionByIdAsync(id);
            if (jobPosition == null)
                return NotFound(new ResultViewModelApi<string>(Error.JobPosition.NOT_FOUNDED,
                    MessageType.WARNING));

            return Ok(new ResultViewModelApi<JobPositionDTO>(jobPosition));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }

    [HttpGet("GetByName/{name}")]
    [ProducesResponseType(typeof(ResultViewModelApi<JobPositionDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByName(string name)
    {
        try
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest(new ResultViewModelApi<string>(Error.JobPosition.NAME_IS_REQUIRED,
                    MessageType.ERROR));

            var jobPosition = await _jobPositionService.GetJobPositionByNameAsync(name);
            
            if (jobPosition == null)
                return NotFound(new ResultViewModelApi<string>(Error.JobPosition.NOT_FOUNDED,
                    MessageType.WARNING));

            return Ok(new ResultViewModelApi<JobPositionDTO>(jobPosition));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }

    [HttpGet("GetByDepartment/{departmentId:guid}")]
    [ProducesResponseType(typeof(ResultViewModelApi<IEnumerable<JobPositionDTO>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByDepartment(Guid departmentId)
    {
        try
        {
            var jobPositions = await _jobPositionService.GetJobPositionByDepartmentIdAsync(departmentId);

            if (_jobPositionService.GetNotifications().Any())
                return BadRequest(new ResultViewModelApi<string>(_jobPositionService.GetNotifications(),
                    MessageType.ERROR));

            return Ok(new ResultViewModelApi<IEnumerable<JobPositionDTO>>(jobPositions));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }

    [HttpGet("GetAll/{skip:int}/{take:int}")]
    [ProducesResponseType(typeof(ResultViewModelApi<IEnumerable<JobPositionDTO>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAll(int skip=0, int take=25)
    {
        try
        {
            var jobPositions = await _jobPositionService.GetAllJobPositionsAsync(skip, take);

            if (_jobPositionService.GetNotifications().Any())
                return BadRequest(new ResultViewModelApi<string>(_jobPositionService.GetNotifications(),
                    MessageType.ERROR));

            return Ok(new ResultViewModelApi<IEnumerable<JobPositionDTO>>(jobPositions));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }

    [HttpPost("Add")]
    [ProducesResponseType(typeof(ResultViewModelApi<bool>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ResultViewModelApi<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddJobPosition([FromBody] JobPositionModel model)
    {
        try
        {
            var result = await _jobPositionService.AddJobPositionAsync(model);

            if (result)
                return StatusCode(201, new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_jobPositionService.GetNotifications(),
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
    public async Task<IActionResult> UpdateJobPosition(Guid id, [FromBody] JobPositionModel model)
    {
        try
        {
            var result = await _jobPositionService.UpdateJobPositionAsync(id, model);

            if (result)
                return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_jobPositionService.GetNotifications(),
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
    public async Task<IActionResult> DeleteJobPosition(Guid id)
    {
        try
        {
            var result = await _jobPositionService.RemoveJobPositionByIdAsync(id);

            if (result)
                return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

            return BadRequest(new ResultViewModelApi<string>(_jobPositionService.GetNotifications(),
                MessageType.ERROR));
        }
        catch (Exception e)
        {
            return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
        }
    }
}