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
    
    #region GetJobPositionByIdAsync

    [Fact]
    public async Task GetJobPositionByIdAsync_ShouldReturnsNullWithError_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var result = await _jobPositionService.GetJobPositionByIdAsync(id);

        // Assert
        Assert.Null(result);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.ID_IS_REQUIRED, _jobPositionService.Notifications.First().Message);
    }

    [Fact]
    public async Task GetJobPositionByIdAsync_ShouldReturnsNull_WithIdNotFounded()
    {
        // Arrange
        var id = Guid.NewGuid();
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(id))
            .ReturnsAsync((JobPosition?)null);

        // Act
        var result = await _jobPositionService.GetJobPositionByIdAsync(id);

        // Assert
        Assert.Null(result);
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
    }

    [Fact]
    public async Task GetJobPositionByIdAsync_ShouldReturnsJobPosition()
    {
        // Arrange
        var id = Guid.NewGuid();
        var department = new Department("Development");
        var jobPosition = new JobPosition("Developer", department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(id))
            .ReturnsAsync(jobPosition);

        // Act
        var result = await _jobPositionService.GetJobPositionByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jobPosition.Name, result.Name);
        Assert.Equal(jobPosition.Id, result.Id);
        Assert.True(_jobPositionService.IsValid);
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
        Assert.Equal("JobPosition.Name", _jobPositionService.Notifications.First().Key);
    }

    [Fact]
    public async Task GetJobPositionByNameAsync_ShouldReturnsNull_WithNameNotFounded()
    {
        // Arrange
        var name = "Developer";
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameAsync(name))
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
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
    }

    #endregion
    
    #region GetAllJobPositionsAsync

    [Fact]
    public async Task GetAllJobPositionsAsync_ShouldReturnsEmptyList()
    {
        // Arrange
        _jobPositionRepositoryMock
            .Setup(x => x.GetAllJobPositionsAsync(0, 25))
            .ReturnsAsync((ICollection<JobPosition>?)null);

        // Act
        var result = await _jobPositionService.GetAllJobPositionsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.IsAssignableFrom<ICollection<JobPositionDTO>>(result);
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
    }

    [Theory]
    [InlineData(0, 25)]
    [InlineData(0, 1)]
    public async Task GetAllJobPositionsAsync_ShouldReturnsList(int skip, int take)
    {
        // Arrange
        var department = new Department("Development");
        var jobPosition = new JobPosition("Developer", department.Id, department);
        var jobPosition2 = new JobPosition("Developer", department.Id, department);
        var jobPosition3 = new JobPosition("Developer", department.Id, department);
        var jobPositions = new List<JobPosition> { jobPosition, jobPosition2, jobPosition3 };
        _jobPositionRepositoryMock
            .Setup(x => x.GetAllJobPositionsAsync(skip, take))
            .ReturnsAsync(jobPositions);

        // Act
        var result = await _jobPositionService.GetAllJobPositionsAsync(skip, take);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.IsAssignableFrom<ICollection<JobPositionDTO>>(result);
        Assert.Equal(jobPositions.Count, result.Count);
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
        Assert.Equal(jobPositions[0].Name, result.ToList()[0].Name);
        Assert.Equal(jobPositions[0].Id, result.ToList()[0].Id);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(-1, -1)]
    public async Task GetAllJobPositionsAsync_ShouldReturnsEmptyList_WithInvalidParameters(int skip, int take)
    {
        // Arrange
        _jobPositionRepositoryMock
            .Setup(x => x.GetAllJobPositionsAsync(skip, take))
            .ReturnsAsync((ICollection<JobPosition>?)null);

        // Act
        var result = await _jobPositionService.GetAllJobPositionsAsync(skip, take);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.IsAssignableFrom<ICollection<JobPositionDTO>>(result);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.INVALID_PAGINATION, _jobPositionService.Notifications.First().Message);
        Assert.Equal("JobPositionService", _jobPositionService.Notifications.First().Key);
    }
    #endregion

    #region AddJobPositionAsync
    [Fact]
    public async Task AddJobPositionAsync_ShouldReturnsFalseWithError_WithInvalidModel()
    {
        // Arrange
        var model = new JobPositionModel("", Guid.NewGuid());

        // Act
        var result = await _jobPositionService.AddJobPositionAsync(model);

        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Single( model.Notifications);
        Assert.Equal(Error.JobPosition.INVALID_NAME, model.Notifications.First().Message);
    }

    [Theory]
    [InlineData("", "Development")]
    [InlineData("D", "Development")]
    [InlineData("Development with more than 30 chars in the name", "Development")]
    public async Task AddJobPositionAsync_ShouldReturnsFalseWithError_WithInvalidJobPositionName(string name, string departmentName)
    {
        // Arrange
        var model = new JobPositionModel(name, Guid.NewGuid());
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

    [Fact]
    public async Task AddJobPositionAsync_ShouldReturnsFalseWithError_WithJobPositionAlreadyExists()
    {
        // Arrange
        var departmentIdd = Guid.NewGuid();
        var model = new JobPositionModel("Developer", departmentIdd);
        var jobPosition = new JobPosition(model.Name, model.DepartmentId);
        _jobPositionRepositoryMock
            .Setup(x => x.AddJobPositionAsync(jobPosition))
            .ReturnsAsync(false);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameAsync(model.Name))
            .ReturnsAsync(jobPosition);
        // Act
        var result = await _jobPositionService.AddJobPositionAsync(model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(result);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.NAME_ALREADY_EXISTS, _jobPositionService.Notifications.First().Message);
        Assert.Equal("JobPosition.Name", _jobPositionService.Notifications.First().Key);
    }

    [Fact]
    public async Task AddJobPositionAsync_ShouldReturnsFalseWithError_WithRepositoryError()
    {
        // Arrange
        var model = new JobPositionModel("Developer", Guid.NewGuid());
        var department = new Department("Development");
        var jobPosition = new JobPosition("Developer", department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.AddJobPositionAsync(jobPosition))
            .ReturnsAsync(false);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameAsync(model.Name))
            .ReturnsAsync((JobPosition?)null);

        // Act
        var result = await _jobPositionService.AddJobPositionAsync(model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(result);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.ERROR_ADDING, _jobPositionService.Notifications.First().Message);
        Assert.Equal("JobPosition", _jobPositionService.Notifications.First().Key);
    }

    [Fact]
    public async Task AddJobPositionAsync_ShouldReturnsTrue()
    {
        // Arrange
        var model = new JobPositionModel("Developer", Guid.NewGuid());
        _jobPositionRepositoryMock
            .Setup(x => x.AddJobPositionAsync(It.IsAny<JobPosition>()))
            .ReturnsAsync(true);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameAsync(model.Name))
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
    
    #region UpdateJobPositionAsync
    [Fact]
    public async Task UpdateJobPositionAsync_ShouldReturnsFalseWithError_WithInvalidModel()
    {
        // Arrange
        var model = new JobPositionModel("", Guid.NewGuid());

        // Act
        var result = await _jobPositionService.UpdateJobPositionAsync(Guid.NewGuid(), model);

        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Single(model.Notifications);
        Assert.Equal(Error.JobPosition.INVALID_NAME, model.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateJobPositionAsync_ShouldReturnsFalseWithError_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;
        var model = new JobPositionModel("Developer",Guid.NewGuid());

        // Act
        var result = await _jobPositionService.UpdateJobPositionAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.ID_IS_REQUIRED, _jobPositionService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateJobPositionAsync_ShouldReturnsFalseWithError_WithJobPositionNotFounded()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new JobPositionModel("Developer",Guid.NewGuid());
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(id))
            .ReturnsAsync((JobPosition?)null);

        // Act
        var result = await _jobPositionService.UpdateJobPositionAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.NOT_FOUNDED, _jobPositionService.Notifications.First().Message);
        Assert.Equal("JobPosition.Id", _jobPositionService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateJobPositionAsync_ShouldReturnsFalseWithError_WithJobPositionAlreadyExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new JobPositionModel("Developer",id);
        var jobPosition = new JobPosition(model.Name,model.DepartmentId);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(id))
            .ReturnsAsync(jobPosition);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameAsync(model.Name))
            .ReturnsAsync(jobPosition);

        // Act
        var result = await _jobPositionService.UpdateJobPositionAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.NAME_ALREADY_EXISTS, _jobPositionService.Notifications.First().Message);
        Assert.Equal("JobPosition.Name", _jobPositionService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateJobPositionAsync_ShouldReturnsFalseWithError_WithRepositoryError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new JobPositionModel("Developer", Guid.NewGuid());
        var department = new Department("Development");
        var jobPosition = new JobPosition("Developer", department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(id))
            .ReturnsAsync(jobPosition);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameAsync(model.Name))
            .ReturnsAsync((JobPosition?)null);
        _jobPositionRepositoryMock
            .Setup(x => x.UpdateJobPositionAsync(jobPosition))
            .ReturnsAsync(false);

        // Act
        var result = await _jobPositionService.UpdateJobPositionAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.ERROR_UPDATING, _jobPositionService.Notifications.First().Message);
        Assert.Equal("JobPosition", _jobPositionService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateJobPositionAsync_ShouldReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new JobPositionModel("Developer", Guid.NewGuid());
        var department = new Department("Development");
        var jobPosition = new JobPosition("Developer", department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(id))
            .ReturnsAsync(jobPosition);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByNameAsync(model.Name))
            .ReturnsAsync((JobPosition?)null);
        _jobPositionRepositoryMock
            .Setup(x => x.UpdateJobPositionAsync( It.IsAny<JobPosition>()))
            .ReturnsAsync(true);

        // Act
        var result = await _jobPositionService.UpdateJobPositionAsync(id, model);

        // Assert
        Assert.True(result);
        Assert.True(model.IsValid);
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
    }
    #endregion
    
    #region RemoveJobPositionAsync
    [Fact]
    public async Task RemoveJobPositionByIdAsync_ShouldReturnsFalseWithError_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var result = await _jobPositionService.RemoveJobPositionByIdAsync(id);

        // Assert
        Assert.False(result);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.ID_IS_REQUIRED, _jobPositionService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task RemoveJobPositionByIdAsync_ShouldReturnsFalseWithError_WithJobPositionNotFounded()
    {
        // Arrange
        var id = Guid.NewGuid();
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(id))
            .ReturnsAsync((JobPosition?)null);

        // Act
        var result = await _jobPositionService.RemoveJobPositionByIdAsync(id);

        // Assert
        Assert.False(result);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.NOT_FOUNDED, _jobPositionService.Notifications.First().Message);
        Assert.Equal("JobPosition.Id", _jobPositionService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task RemoveJobPositionByIdAsyncShouldReturnsFalseWithError_WithRepositoryError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var department = new Department("Development");
        var jobPosition = new JobPosition("Developer", department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(id))
            .ReturnsAsync(jobPosition);
        _jobPositionRepositoryMock
            .Setup(x => x.RemoveJobPositionAsync(jobPosition))
            .ReturnsAsync(false);

        // Act
        var result = await _jobPositionService.RemoveJobPositionByIdAsync(id);

        // Assert
        Assert.False(result);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.ERROR_REMOVING, _jobPositionService.Notifications.First().Message);
        Assert.Equal("JobPosition", _jobPositionService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task RemoveJobPositionByIdAsync_ShouldReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var department = new Department("Development");
        var jobPosition = new JobPosition("Developer", department.Id, department);
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(id))
            .ReturnsAsync(jobPosition);
        _jobPositionRepositoryMock
            .Setup(x => x.RemoveJobPositionAsync(jobPosition))
            .ReturnsAsync(true);

        // Act
        var result = await _jobPositionService.RemoveJobPositionByIdAsync(id);

        // Assert
        Assert.True(result);
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
    }
    #endregion

    #region Internal Methods

    [Fact]
    public Task ValidateIdForSearch_ShouldReturnsFalseWithError_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var result = _jobPositionService.ValidateIdForSearch(id);

        // Assert
        Assert.False(result);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.ID_IS_REQUIRED, _jobPositionService.Notifications.First().Message);
        Assert.Equal("JobPosition.Id", _jobPositionService.Notifications.First().Key);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ValidateIdForSearch_ShouldReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var result = _jobPositionService.ValidateIdForSearch(id);

        // Assert
        Assert.True(result);
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
    }

    [Fact]
    public async Task ValidateNameForSearch_ShouldReturnsFalseWithError_WithInvalidName()
    {
        // Arrange
        var name = "";

        // Act
        var result = _jobPositionService.ValidateNameForSearch(name);

        // Assert
        Assert.False(result);
        Assert.False(_jobPositionService.IsValid);
        Assert.Single(_jobPositionService.Notifications);
        Assert.Equal(Error.JobPosition.NAME_IS_REQUIRED, _jobPositionService.Notifications.First().Message);
        Assert.Equal("JobPosition.Name", _jobPositionService.Notifications.First().Key);
    }

    [Fact]
    public async Task ValidateNameForSearch_ShouldReturnsTrue()
    {
        // Arrange
        var name = "Company";

        // Act
        var result = _jobPositionService.ValidateNameForSearch(name);

        // Assert
        Assert.True(result);
        Assert.True(_jobPositionService.IsValid);
        Assert.Empty(_jobPositionService.Notifications);
    }

    #endregion
}
