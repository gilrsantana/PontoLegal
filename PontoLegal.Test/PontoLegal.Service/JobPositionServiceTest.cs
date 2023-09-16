﻿using PontoLegal.Service.DTOs;

namespace PontoLegal.Test.PontoLegal.Service;

public class JobPositionServiceTest
{
    private readonly Mock<IJobPositionRepository> _jobPositionRepositoryMock;
    private readonly JobPositionService _jobPositionService;

    public JobPositionServiceTest()
    {
        _jobPositionRepositoryMock = new Mock<IJobPositionRepository>();
        _jobPositionService = new JobPositionService(_jobPositionRepositoryMock.Object);
    }

    #region AddJobPositionAsync
    [Fact]
    public async Task AddJobPositionAsync_ShouldReturnsFalseWithError_WithInvalidModel()
    {
        // Arrange
        var model = new JobPositionModel("", new DepartmentModel(""));

        // Act
        var result = await _jobPositionService.AddJobPositionAsync(model);

        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.False(_jobPositionService.IsValid);
        Assert.Equal(2, _jobPositionService.Notifications.Count);
        Assert.Equal(2, model.Notifications.Count);
        Assert.Equal(Error.JobPosition.INVALID_NAME, model.Notifications.First().Message);
        Assert.Equal(Error.Department.INVALID_NAME, model.Notifications.Last().Message);
    }

    [Theory]
    [InlineData("", "Development")]
    [InlineData("D", "Development")]
    [InlineData("Development with more than 30 chars in the name", "Development")]
    public async Task AddJobPositionAsync_ShouldReturnsFalseWithError_WithInvalidJobPositionName(string name, string departmentName)
    {
        // Arrange
        var departmentModel = new DepartmentModel(departmentName);
        var model = new JobPositionModel(name, departmentModel);
        var department = new Department(departmentName);
        var jobPosition = new JobPosition(name, department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.AddJobPositionAsync(jobPosition))
            .ReturnsAsync(false);

        // Act
        var result = await _jobPositionService.AddJobPositionAsync(model);

        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Equal(Error.JobPosition.INVALID_NAME, model.Notifications.First().Message);
    }

    [Theory]
    [InlineData("Development", "")]
    [InlineData("Development", "D")]
    [InlineData("Development", "Department with more than 30 chars in the name")]
    public async Task AddJobPositionAsync_ShouldReturnsFalseWithError_WithInvalidDepartmentName(string name, string departmentName)
    {
        // Arrange
        var departmentModel = new DepartmentModel(departmentName);
        var model = new JobPositionModel(name, departmentModel);
        var department = new Department(departmentName);
        var jobPosition = new JobPosition(name, department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.AddJobPositionAsync(jobPosition))
            .ReturnsAsync(false);

        // Act
        var result = await _jobPositionService.AddJobPositionAsync(model);

        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Equal(Error.Department.INVALID_NAME, model.Notifications.First().Message);
    }

    [Fact]
    public async Task AddJobPositionAsync_ShouldReturnsFalseWithError_WithJobPositionAlreadyExists()
    {
        // Arrange
        var departmentModel = new DepartmentModel("Development");
        var model = new JobPositionModel("Developer", departmentModel);
        var department = new Department("Development");
        var jobPosition = new JobPosition("Developer", department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.AddJobPositionAsync(jobPosition))
            .ReturnsAsync(false);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameIncludeDepartmentAsync(model.Name))
            .ReturnsAsync(jobPosition);
        // Act
        var result = await _jobPositionService.AddJobPositionAsync(model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(result);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.NAME_ALREADY_EXISTS, _jobPositionService.Notifications.First().Message);
    }

    [Fact]
    public async Task AddJobPositionAsync_ShouldReturnsFalseWithError_WithRepositoryError()
    {
        // Arrange
        var departmentModel = new DepartmentModel("Development");
        var model = new JobPositionModel("Developer", departmentModel);
        var department = new Department("Development");
        var jobPosition = new JobPosition("Developer", department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.AddJobPositionAsync(jobPosition))
            .ReturnsAsync(false);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameIncludeDepartmentAsync(model.Name))
            .ReturnsAsync((JobPosition?)null);

        // Act
        var result = await _jobPositionService.AddJobPositionAsync(model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(result);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.ERROR_ADDING, _jobPositionService.Notifications.First().Message);
    }

    [Fact]
    public async Task AddJobPositionAsync_ShouldReturnsTrue()
    {
        // Arrange
        var departmentModel = new DepartmentModel("Development");
        var model = new JobPositionModel("Developer", departmentModel);
        _jobPositionRepositoryMock
            .Setup(x => x.AddJobPositionAsync(It.IsAny<JobPosition>()))
            .ReturnsAsync(true);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameIncludeDepartmentAsync(model.Name))
            .ReturnsAsync((JobPosition?)null);

        // Act
        var result = await _jobPositionService.AddJobPositionAsync(model);

        // Assert
        Assert.True(model.IsValid);
        Assert.True(_jobPositionService.IsValid);
        Assert.True(result);
        Assert.Empty(_jobPositionService.Notifications);
    }


    #endregion

    #region GetJobPositionByNameAsync

    [Fact]
    public async Task GetJobPositionByNameAsync_ShouldReturnsNullWithError_WithInvalidName()
    {
        // Arrange
        var name = "";

        // Act
        var result = await _jobPositionService.GetJobPositionByNameAsync(name);

        // Assert
        Assert.Null(result);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.NAME_IS_REQUIRED, _jobPositionService.Notifications.First().Message);
    }

    [Fact]
    public async Task GetJobPositionByNameAsync_ShouldReturnsNull_WithNameNotFounded()
    {
        // Arrange
        var name = "Developer";
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameIncludeDepartmentAsync(name))
            .ReturnsAsync((JobPosition?)null);

        // Act
        var result = await _jobPositionService.GetJobPositionByNameAsync(name);

        // Assert
        Assert.Null(result);
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
    }

    [Fact]
    public async Task GetJobPositionByNameAsync_ShouldReturnsJobPosition()
    {
        // Arrange
        var name = "Developer";
        var department = new Department("Development");
        var jobPosition = new JobPosition(name, department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameAsync(name))
            .ReturnsAsync(jobPosition);

        // Act
        var result = await _jobPositionService.GetJobPositionByNameAsync(name);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jobPosition.Name, result.Name);
        Assert.Equal(jobPosition.Id, result.Id);
        Assert.True(string.IsNullOrEmpty(result.Department.Name));
        Assert.Equal(Guid.Empty, result.Department.Id);
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
    }

    [Fact]
    public async Task GetJobPositionByNameIncludeDepartmentAsync_ShouldReturnsJobPosition_WithDepartment()
    {
        // Arrange
        var name = "Developer";
        var department = new Department("Development");
        var jobPosition = new JobPosition(name, department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameIncludeDepartmentAsync(name))
            .ReturnsAsync(jobPosition);

        // Act
        var result = await _jobPositionService.GetJobPositionByNameIncludeDepartmentAsync(name);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jobPosition.Name, result.Name);
        Assert.Equal(jobPosition.Id, result.Id);
        Assert.Equal(jobPosition.Department.Name, result.Department.Name);
        Assert.Equal(jobPosition.Department.Id, result.Department.Id);
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
    }

    [Fact]
    public async Task GetJobPositionByNameIncludeDepartmentAsync_ShouldReturnsNull_WithNameNotFounded()
    {
        // Arrange
        var name = "Developer";
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameIncludeDepartmentAsync(name))
            .ReturnsAsync((JobPosition?)null);

        // Act
        var result = await _jobPositionService.GetJobPositionByNameIncludeDepartmentAsync(name);

        // Assert
        Assert.Null(result);
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
    }
    #endregion
}
