﻿using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Models;
using PontoLegal.Service.Interfaces;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service;

public class DepartmentService : BaseService, IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentDTO?> GetDepartmentByIdAsync(Guid departmentId)
    {
        var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
        return department == null
            ? null
            : new DepartmentDTO { Id = department.Id, Name = department.Name };
    }
    
    public async Task<DepartmentDTO?> GetDepartmentByNameAsync(string departmentName)
    {
        var department = await _departmentRepository.GetDepartmentByNameAsync(departmentName);
            
        return department == null 
            ? null 
            : new DepartmentDTO { Id = department.Id, Name = department.Name };
    }
    
    public async Task<ICollection<DepartmentDTO>> GetAllDepartmentsAsync(int skip=0, int take=25)
    {
        if (skip < 0 || take < 1)
        {
            AddNotification("DepartmentService", Error.Department.INVALID_PAGINATION);
            return new List<DepartmentDTO>();
        }

        var result = await _departmentRepository.GetAllDepartmentsAsync(skip, take);
        if (result != null)
        {
            var departments = new List<DepartmentDTO>();
            foreach (var department in result)
            {
                departments.Add(new DepartmentDTO { Id = department.Id, Name = department.Name });
            }
            return departments;
        }
        
        return new List<DepartmentDTO>();
    }
    
    public async Task<bool> AddDepartmentAsync(DepartmentModel model)
    {
        if (!model.IsValid)
        {
            AddNotifications(model.Notifications);
            return false;
        }

        var modelExists = await GetDepartmentByNameAsync(model.Name);

        if (modelExists != null)
        {
            AddNotification("DepartmentService.Name", Error.Department.NAME_ALREADY_EXISTS);
            return false;

        }

        var department = new Department(model.Name);

        var result = await _departmentRepository.AddDepartmentAsync(department);
        if (result)
            return true;

        AddNotification("DepartmentService", Error.Department.ERROR_ADDING);
        return false;
    }
    
    public async Task<bool> UpdateDepartmentAsync(Guid id, DepartmentModel model)
    {
        if (!model.IsValid)
        {
            foreach (var notification in model.Notifications)
            {
                AddNotification(notification);
            }
            return false;
        }

        var nameExists = await GetDepartmentByNameAsync(model.Name);
        var idExists = await GetDepartmentByIdAsync(id);
           
        if (idExists == null)
        {
            AddNotification("DepartmentService.Id", Error.Department.DEPARTMENT_NOT_FOUNDED);
            return false;
        }

        if (nameExists != null &&
            nameExists.Id.ToString() != idExists.Id.ToString())
        {
            AddNotification("DepartmentService.Name", Error.Department.NAME_ALREADY_EXISTS);
            return false;
        }
        
        var department = new Department(id, model.Name);

        var result = await _departmentRepository.UpdateDepartmentAsync(department);
            
        if (result) return true;

        AddNotification("DepartmentService", Error.Department.ERROR_UPDATING);
        return false;
    }

    public async Task<bool> RemoveDepartmentByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            AddNotification("DepartmentService.Id", Error.Department.INVALID_ID);
            return false;
        }

        var department = await GetDepartmentByIdAsync(id);
        if (department == null)
        {
            AddNotification("DepartmentService.Id", Error.Department.DEPARTMENT_NOT_FOUNDED);
            return false;
        }

        var result = await _departmentRepository.RemoveDepartmentByIdAsync(id);
        if (result)
            return true;

        AddNotification("DepartmentService", Error.Department.ERROR_REMOVING);
        return false;
    }
}