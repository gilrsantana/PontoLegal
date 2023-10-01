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
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("GetById/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResultViewModelApi<string>(Error.Department.INVALID_ID, MessageType.ERROR));
                
                var department = await _departmentService.GetDepartmentByIdAsync(id);
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
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return BadRequest(new ResultViewModelApi<string>(Error.Department.INVALID_NAME, MessageType.ERROR));
                
                var department = await _departmentService.GetDepartmentByNameAsync(name);
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
        public async Task<IActionResult> GetAll(int skip=0, int take=25)
        {
            try
            {
                if (skip < 0 || take < 1)
                {
                    return BadRequest(new ResultViewModelApi<string>(Error.Department.INVALID_PAGINATION, MessageType.ERROR));
                }

                var departments = await _departmentService.GetAllDepartmentsAsync(skip, take);

                return Ok(new ResultViewModelApi<ICollection<DepartmentDTO>?>(departments));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment(DepartmentModel model)
        {
            try
            {
                if (!model.IsValid)
                    return BadRequest(
                        new ResultViewModelApi<string>(model.Notifications.Select(n => n.Message).ToList(), MessageType.ERROR));
                
                var result = await _departmentService.AddDepartmentAsync(model);
                if (result)
                    return StatusCode(201,new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));
                

                return BadRequest(new ResultViewModelApi<string>(Error.Department.ERROR_ADDING, MessageType.ERROR));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }

        [HttpPut("Update/{id:guid}")]
        public async Task<IActionResult> UpdateDepartment(Guid id, DepartmentModel model)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResultViewModelApi<string>(Error.Department.INVALID_ID, MessageType.ERROR));

                var result = await _departmentService.UpdateDepartmentAsync(id, model);
                if (result)
                    return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

                return BadRequest(new ResultViewModelApi<string>(Error.Department.ERROR_UPDATING, MessageType.ERROR));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }

        [HttpDelete("Delete/{id:guid}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResultViewModelApi<string>(Error.Department.INVALID_ID, MessageType.ERROR));

                var result = await _departmentService.RemoveDepartmentByIdAsync(id);
                if (result)
                    return Ok(new ResultViewModelApi<bool>(result, new List<MessageModel> { new("Success") }));

                return BadRequest(new ResultViewModelApi<string>(Error.Department.ERROR_REMOVING, MessageType.ERROR));
            }
            catch (Exception e)
            {
                return BadRequest(new ResultViewModelApi<string>(e.Message, MessageType.ERROR));
            }
        }
    }
}
