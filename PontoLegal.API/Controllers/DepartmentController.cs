using Microsoft.AspNetCore.Mvc;
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
                {
                    return BadRequest(Error.Department.INVALID_ID);
                }
                var department = await _departmentService.GetDepartmentByIdAsync(id);
                if (department == null)
                {
                    return NotFound(Error.Department.DEPARTMENT_NOT_FOUNDED);
                }

                return Ok(department);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest(Error.Department.INVALID_NAME);
                }

                var department = await _departmentService.GetDepartmentByNameAsync(name);
                if (department == null)
                {
                    return NotFound(Error.Department.DEPARTMENT_NOT_FOUNDED);
                }

                return Ok(department);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        [HttpGet("GetAll/{skip:int}/{take:int}")]
        public async Task<IActionResult> GetAll(int skip=0, int take=25)
        {
            try
            {
                if (skip < 0 || take < 0)
                {
                    return BadRequest(Error.Department.INVALID_PAGINATION);
                }

                var departments = await _departmentService.GetAllDepartmentsAsync(skip, take);
                if (departments == null)
                {
                    return NotFound(Error.Department.DEPARTMENT_NOT_FOUNDED);
                }

                return Ok(departments);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment(DepartmentModel model)
        {
            try
            {
                var result = await _departmentService.AddDepartmentAsync(model);
                if (result)
                {
                    return Ok();
                }

                return BadRequest(Error.Department.ERROR_ADDING);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        [HttpPut("Update/{id:guid}")]
        public async Task<IActionResult> UpdateDepartment(Guid id, DepartmentModel model)
        {
            try
            {
                var result = await _departmentService.UpdateDepartmentAsync(id, model);
                if (result)
                {
                    return Ok();
                }

                return BadRequest(Error.Department.ERROR_UPDATING);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        [HttpDelete("Delete/{id:guid}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            try
            {
                var result = await _departmentService.RemoveDepartmentByIdAsync(id);
                if (result)
                {
                    return Ok();
                }

                return BadRequest(Error.Department.ERROR_REMOVING);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
