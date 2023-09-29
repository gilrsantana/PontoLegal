

using NuGet.Frameworks;

namespace PontoLegal.Test.PontoLegal.Service;

public class WorkingDayServiceTest
{
    private readonly Mock<IWorkingDayRepository> _workingDayRepositoryMock;
    private readonly WorkingDayService _workingDayService;

    public WorkingDayServiceTest()
    {
        _workingDayRepositoryMock = new Mock<IWorkingDayRepository>();
        _workingDayService = new WorkingDayService(_workingDayRepositoryMock.Object);
    }
    
    #region GetWorkingDayByNameAsync
    [Fact]
    public async Task GetWorkingDayByNameAsync_ShouldReturnsNull_WithInvalidName()
    {
        // Arrange
        var name = "";
        
        // Act
        var result = await _workingDayService.GetWorkingDayByNameAsync(name);
        
        // Assert
        Assert.Null(result);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.NAME_IS_REQUIRED, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task GetWorkingDayByNameAsync_ShouldReturnsNull_WithNotExistingWorkingDay()
    {
        // Arrange
        var name = "Name of Working Day";
        
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((WorkingDay?)null);
        
        // Act
        var result = await _workingDayService.GetWorkingDayByNameAsync(name);
        
        // Assert
        Assert.Null(result);
        Assert.Empty(_workingDayService.Notifications);
    }
    
    [Fact]
    public async Task GetWorkingDayByNameAsync_ShouldReturnsWorkingDayDTO()
    {
        // Arrange
        var name = "Name of Working Day";
        
        var workingDay = new WorkingDay(
            "Name of Working Day", 
            WorkingDayType.NINE_HOURS, 
            new TimeOnly(7, 0), 
            new TimeOnly(11, 0), 
            new TimeOnly(12, 0), 
            new TimeOnly(16, 0));
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(workingDay);
        
        // Act
        var result = await _workingDayService.GetWorkingDayByNameAsync(name);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<WorkingDayDTO?>(result);
        Assert.Empty(_workingDayService.Notifications);
        Assert.Equal(workingDay.Id, result.Id);
        Assert.Equal(workingDay.Name, result.Name);
        Assert.Equal(workingDay.Type, result.Type);
        Assert.Equal(workingDay.StartWork, result.StartWork);
        Assert.Equal(workingDay.StartBreak, result.StartBreak);
        Assert.Equal(workingDay.EndBreak, result.EndBreak);
        Assert.Equal(workingDay.EndWork, result.EndWork);
    }
    
    #endregion
    
    #region GetWorkingDayByIdAsync
    [Fact]
    public async Task GetWorkingDayByIdAsync_ShouldReturnsNull_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;
        
        // Act
        var result = await _workingDayService.GetWorkingDayByIdAsync(id);
        
        // Assert
        Assert.Null(result);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.ID_IS_REQUIRED, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task GetWorkingDayByIdAsync_ShouldReturnsNull_WithNotExistingWorkingDay()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((WorkingDay?)null);
        
        // Act
        var result = await _workingDayService.GetWorkingDayByIdAsync(id);
        
        // Assert
        Assert.Null(result);
        Assert.Empty(_workingDayService.Notifications);
    }
    
    [Fact]
    public async Task GetWorkingDayByIdAsync_ShouldReturnsWorkingDayDTO()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        var workingDay = new WorkingDay(
            "Name of Working Day", 
            WorkingDayType.NINE_HOURS, 
            new TimeOnly(7, 0), 
            new TimeOnly(11, 0), 
            new TimeOnly(12, 0), 
            new TimeOnly(16, 0));
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(workingDay);
        
        // Act
        var result = await _workingDayService.GetWorkingDayByIdAsync(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<WorkingDayDTO?>(result);
        Assert.Empty(_workingDayService.Notifications);
        Assert.Equal(workingDay.Id, result.Id);
        Assert.Equal(workingDay.Name, result.Name);
        Assert.Equal(workingDay.Type, result.Type);
        Assert.Equal(workingDay.StartWork, result.StartWork);
        Assert.Equal(workingDay.StartBreak, result.StartBreak);
        Assert.Equal(workingDay.EndBreak, result.EndBreak);
        Assert.Equal(workingDay.EndWork, result.EndWork);
    }
    #endregion
    
    #region GetAllWorkingDaysAsync
    
    [Theory]
    [InlineData(-1, 25)]
    [InlineData(0, 0)]
    [InlineData(0, -1)]
    public async Task GetAllWorkingDaysAsync_ShouldReturnsEmptyList_WithInvalidSkipOrTake(int skip, int take)
    {
        // Arrange
        // Act
        var result = await _workingDayService.GetAllWorkingDaysAsync(skip, take);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<WorkingDayDTO>>(result);
        Assert.Empty(result);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.INVALID_PAGINATION, _workingDayService.Notifications.First().Message);
        Assert.Equal("WorkingDayService", _workingDayService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task GetAllWorkingDaysAsync_ShouldReturnsEmptyList()
    {
        // Arrange
        var workingDays = new List<WorkingDay>();
        _workingDayRepositoryMock
            .Setup(x => x.GetAllWorkingDaysAsync<WorkingDay>(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(workingDays);
        // Act
        var result = await _workingDayService.GetAllWorkingDaysAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<WorkingDayDTO>>(result);
        Assert.Empty(result);
        Assert.Empty(_workingDayService.Notifications);
    }
    
    [Fact]
    public async Task GetAllWorkingDaysAsync_ShouldReturnsWorkingDayDTOList()
    {
        // Arrange
        var workingDays = new List<WorkingDay>
        {
            new WorkingDay(
                "Name of Working Day 1", 
                WorkingDayType.NINE_HOURS, 
                new TimeOnly(7, 0), 
                new TimeOnly(11, 0), 
                new TimeOnly(12, 0), 
                new TimeOnly(16, 0)),
            new WorkingDay(
                "Name of Working Day 2", 
                WorkingDayType.NINE_HOURS, 
                new TimeOnly(7, 0), 
                new TimeOnly(11, 0), 
                new TimeOnly(12, 0), 
                new TimeOnly(16, 0))
        };
        _workingDayRepositoryMock
            .Setup(x => x.GetAllWorkingDaysAsync<WorkingDay>(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(workingDays);
        // Act
        var result = await _workingDayService.GetAllWorkingDaysAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<WorkingDayDTO>>(result);
        Assert.Equal(workingDays.Count, result.Count);
        Assert.Empty(_workingDayService.Notifications);
        Assert.Equal(workingDays[0].Id, result.ToList()[0].Id);
        Assert.Equal(workingDays[0].Name, result.ToList()[0].Name);
        Assert.Equal(workingDays[0].Type, result.ToList()[0].Type);
        Assert.Equal(workingDays[0].StartWork, result.ToList()[0].StartWork);
        Assert.Equal(workingDays[0].StartBreak, result.ToList()[0].StartBreak);
        Assert.Equal(workingDays[0].EndBreak, result.ToList()[0].EndBreak);
        Assert.Equal(workingDays[0].EndWork, result.ToList()[0].EndWork);
        Assert.Equal(workingDays[1].Id, result.ToList()[1].Id);
        Assert.Equal(workingDays[1].Name, result.ToList()[1].Name);
        Assert.Equal(workingDays[1].Type, result.ToList()[1].Type);
        Assert.Equal(workingDays[1].StartWork, result.ToList()[1].StartWork);
        Assert.Equal(workingDays[1].StartBreak, result.ToList()[1].StartBreak);
        Assert.Equal(workingDays[1].EndBreak, result.ToList()[1].EndBreak);
        Assert.Equal(workingDays[1].EndWork, result.ToList()[1].EndWork);

    }
    
    #endregion
    
    #region AddWorkingDayAsync
    [Theory]
    [InlineData("")]
    [InlineData("C")]
    [InlineData("Name of Working Day with more than 30 characters")]
    public async Task AddWorkingDayAsync_ShouldReturnsFalseWithError_WithInvalidName(string name)
    {
        // Arrange
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);
        
        // Act
        var result = await _workingDayService.AddWorkingDayAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.INVALID_NAME, model.Notifications.First().Message);
        Assert.Equal(Error.WorkingDay.INVALID_NAME, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddWorkingDayAsync_ShouldReturnsFalseWithError_WithInvalidStartWork()
    {
        // Arrange
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(6, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);
        
        // Act
        var result = await _workingDayService.AddWorkingDayAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.INVALID_START_WORK, model.Notifications.First().Message);
        Assert.Equal(Error.WorkingDay.INVALID_START_WORK, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddWorkingDayAsync_ShouldReturnsFalseWithError_WithInvalidStartBreak()
    {
        // Arrange
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(10, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);
        
        // Act
        var result = await _workingDayService.AddWorkingDayAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.INVALID_START_BREAK, model.Notifications.First().Message);
        Assert.Equal(Error.WorkingDay.INVALID_START_BREAK, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddWorkingDayAsync_ShouldReturnsFalseWithError_WithInvalidEndBreak()
    {
        // Arrange
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(17, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);
        
        // Act
        var result = await _workingDayService.AddWorkingDayAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.INVALID_END_BREAK, model.Notifications.First().Message);
        Assert.Equal(Error.WorkingDay.INVALID_END_BREAK, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddWorkingDayAsync_ShouldReturnsFalseWithError_WithInvalidType()
    {
        // Arrange
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.EIGHT_HOURS, startWork, startBreak, endBreak, endWork);
        
        // Act
        var result = await _workingDayService.AddWorkingDayAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.INVALID_TYPE, model.Notifications.First().Message);
        Assert.Equal(Error.WorkingDay.INVALID_TYPE, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddWorkingDayAsync_ShouldReturnsFalseWithError_WithExistingWorkingDay()
    {
        // Arrange
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);
        
        var workingDay = new WorkingDay(
            "Name of Working Day", 
            WorkingDayType.NINE_HOURS, 
            new TimeOnly(7, 0), 
            new TimeOnly(11, 0), 
            new TimeOnly(12, 0), 
            new TimeOnly(16, 0));
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(workingDay);
        
        // Act
        var result = await _workingDayService.AddWorkingDayAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.NAME_ALREADY_EXISTS, _workingDayService.Notifications.First().Message);
        Assert.Equal("WorkingDay.Name", _workingDayService.Notifications.First().Key);
    }

    [Fact]
    public async Task AddWorkingDayAsync_ShouldReturnsFalseWithError_WithRepositoryError()
    {
        // Arrange
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);
        
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((WorkingDay?)null);
        _workingDayRepositoryMock
            .Setup(x => x.AddWorkingDayAsync(It.IsAny<WorkingDay>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _workingDayService.AddWorkingDayAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.ERROR_ADDING, _workingDayService.Notifications.First().Message);
        Assert.Equal("WorkingDay", _workingDayService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task AddWorkingDayAsync_ShouldReturnsTrue()
    {
        // Arrange
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);
        
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((WorkingDay?)null);
        _workingDayRepositoryMock
            .Setup(x => x.AddWorkingDayAsync(It.IsAny<WorkingDay>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _workingDayService.AddWorkingDayAsync(model);
        
        // Assert
        Assert.True(result);
        Assert.True(model.IsValid);
        Assert.Empty(_workingDayService.Notifications);
    }
    #endregion
    
    #region UpdateWorkingDayAsync
    [Fact]
    public async Task UpdateWorkingDayAsync_ShouldReturnsFalseWithError_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);
        
        // Act
        var result = await _workingDayService.UpdateWorkingDayAsync(id, model);
        
        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.ID_IS_REQUIRED, _workingDayService.Notifications.First().Message);
    }    
    
    [Fact]
    public async Task UpdateWorkingDayAsync_ShouldReturnsFalseWithError_WithInvalidName()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);
        
        // Act
        var result = await _workingDayService.UpdateWorkingDayAsync(id, model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.INVALID_NAME, model.Notifications.First().Message);
        Assert.Equal(Error.WorkingDay.INVALID_NAME, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateWorkingDayAsync_ShouldReturnsFalseWithError_WithInvalidStartWork()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(6, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);

        // Act
        var result = await _workingDayService.UpdateWorkingDayAsync(id, model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.INVALID_START_WORK, model.Notifications.First().Message);
        Assert.Equal(Error.WorkingDay.INVALID_START_WORK, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateWorkingDayAsync_ShouldReturnsFalseWithError_WithInvalidStartBreak()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(10, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);

        // Act
        var result = await _workingDayService.UpdateWorkingDayAsync(id, model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.INVALID_START_BREAK, model.Notifications.First().Message);
        Assert.Equal(Error.WorkingDay.INVALID_START_BREAK, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateWorkingDayAsync_ShouldReturnsFalseWithError_WithInvalidEndBreak()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(17, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);

        // Act
        var result = await _workingDayService.UpdateWorkingDayAsync(id, model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.INVALID_END_BREAK, model.Notifications.First().Message);
        Assert.Equal(Error.WorkingDay.INVALID_END_BREAK, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateWorkingDayAsync_ShouldReturnsFalseWithError_WithInvalidType()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.EIGHT_HOURS, startWork, startBreak, endBreak, endWork);

        // Act
        var result = await _workingDayService.UpdateWorkingDayAsync(id, model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.INVALID_TYPE, model.Notifications.First().Message);
        Assert.Equal(Error.WorkingDay.INVALID_TYPE, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateWorkingDayAsync_ShouldReturnsFalseWithError_WithNotExistingWorkingDay()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);

        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((WorkingDay?)null);
        
        // Act
        var result = await _workingDayService.UpdateWorkingDayAsync(id, model);
        
        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.NOT_FOUNDED, _workingDayService.Notifications.First().Message);
        Assert.Equal("WorkingDay.Id", _workingDayService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateWorkingDayAsync_ShouldReturnsFalseWithError_WithExistingWorkingDay()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);

        var workingDay = new WorkingDay(
            "Name of Working Day", 
            WorkingDayType.NINE_HOURS, 
            new TimeOnly(7, 0), 
            new TimeOnly(11, 0), 
            new TimeOnly(12, 0), 
            new TimeOnly(16, 0));
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(workingDay);
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(workingDay);
        
        // Act
        var result = await _workingDayService.UpdateWorkingDayAsync(id, model);
        
        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.NAME_ALREADY_EXISTS, _workingDayService.Notifications.First().Message);
        Assert.Equal("WorkingDay.Name", _workingDayService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateWorkingDayAsync_ShouldReturnsFalseWithError_WithRepositoryError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);

        var workingDay = new WorkingDay(
            "New Working Day", 
            WorkingDayType.NINE_HOURS, 
            new TimeOnly(7, 0), 
            new TimeOnly(11, 0), 
            new TimeOnly(12, 0), 
            new TimeOnly(16, 0));
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(workingDay);
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((WorkingDay?)null);
        _workingDayRepositoryMock
            .Setup(x => x.UpdateWorkingDayAsync(It.IsAny<Guid>(), It.IsAny<WorkingDay>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _workingDayService.UpdateWorkingDayAsync(id, model);
        
        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.ERROR_UPDATING, _workingDayService.Notifications.First().Message);
        Assert.Equal("WorkingDay", _workingDayService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateWorkingDayAsync_ShouldReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Name of Working Day";
        var startWork = new TimeOnly(7, 0);
        var startBreak = new TimeOnly(11, 0);
        var endBreak = new TimeOnly(12, 0);
        var endWork = new TimeOnly(16, 0);
        var model = new WorkingDayModel(name, WorkingDayType.NINE_HOURS, startWork, startBreak, endBreak, endWork);

        var workingDay = new WorkingDay(
            "New Working Day", 
            WorkingDayType.NINE_HOURS, 
            new TimeOnly(7, 0), 
            new TimeOnly(11, 0), 
            new TimeOnly(12, 0), 
            new TimeOnly(16, 0));
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(workingDay);
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((WorkingDay?)null);
        _workingDayRepositoryMock
            .Setup(x => x.UpdateWorkingDayAsync(It.IsAny<Guid>(), It.IsAny<WorkingDay>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _workingDayService.UpdateWorkingDayAsync(id, model);
        
        // Assert
        Assert.True(result);
        Assert.True(model.IsValid);
        Assert.Empty(_workingDayService.Notifications);
    }
    
    #endregion
    
    #region RemoveWorkingDayByIdAsync
    [Fact]
    public async Task RemoveWorkingDayByIdAsync_ShouldReturnsFalse_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;
        
        // Act
        var result = await _workingDayService.RemoveWorkingDayByIdAsync(id);
        
        // Assert
        Assert.False(result);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.ID_IS_REQUIRED, _workingDayService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task RemoveWorkingDayByIdAsync_ShouldReturnsFalse_WithNotExistingWorkingDay()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((WorkingDay?)null);
        
        // Act
        var result = await _workingDayService.RemoveWorkingDayByIdAsync(id);
        
        // Assert
        Assert.False(result);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.NOT_FOUNDED, _workingDayService.Notifications.First().Message);
        Assert.Equal("WorkingDay.Id", _workingDayService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task RemoveWorkingDayByIdAsync_ShouldReturnsFalseWithError_WithRepositoryError()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        var workingDay = new WorkingDay(
            "Name of Working Day", 
            WorkingDayType.NINE_HOURS, 
            new TimeOnly(7, 0), 
            new TimeOnly(11, 0), 
            new TimeOnly(12, 0), 
            new TimeOnly(16, 0));
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(workingDay);
        _workingDayRepositoryMock
            .Setup(x => x.RemoveWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _workingDayService.RemoveWorkingDayByIdAsync(id);
        
        // Assert
        Assert.False(result);
        Assert.Single(_workingDayService.Notifications);
        Assert.Equal(Error.WorkingDay.ERROR_REMOVING, _workingDayService.Notifications.First().Message);
        Assert.Equal("WorkingDay", _workingDayService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task RemoveWorkingDayByIdAsync_ShouldReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        var workingDay = new WorkingDay(
            "Name of Working Day", 
            WorkingDayType.NINE_HOURS, 
            new TimeOnly(7, 0), 
            new TimeOnly(11, 0), 
            new TimeOnly(12, 0), 
            new TimeOnly(16, 0));
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(workingDay);
        _workingDayRepositoryMock
            .Setup(x => x.RemoveWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _workingDayService.RemoveWorkingDayByIdAsync(id);
        
        // Assert
        Assert.True(result);
        Assert.Empty(_workingDayService.Notifications);
    }
    
    #endregion
}