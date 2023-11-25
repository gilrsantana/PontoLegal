namespace PontoLegal.Test.PontoLegal.Service;

public class TimeClockServiceTest
{
    private readonly Mock<ITimeClockRepository> _timeClockRepositoryMock;
    private readonly TimeClockService _timeClockService;

    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly EmployeeService _employeeService;

    private readonly Mock<IWorkingDayRepository> _workingDayRepositoryMock;
    private readonly WorkingDayService _workingDayService;

    private readonly Mock<IJobPositionRepository> _jobPositionRepositoryMock;
    private readonly JobPositionService _jobPositionService;

    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly CompanyService _companyService;


    public TimeClockServiceTest()
    {
        _jobPositionRepositoryMock = new Mock<IJobPositionRepository>();
        _jobPositionService = new JobPositionService(_jobPositionRepositoryMock.Object);
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _companyService = new CompanyService(_companyRepositoryMock.Object);
        _workingDayRepositoryMock = new Mock<IWorkingDayRepository>();
        _workingDayService = new WorkingDayService(_workingDayRepositoryMock.Object);
        _employeeService = new EmployeeService(_employeeRepositoryMock.Object, _jobPositionService, _companyService, _workingDayService);
        _timeClockRepositoryMock = new Mock<ITimeClockRepository>();
        // _timeClockService = new TimeClockService(_timeClockRepositoryMock.Object, _employeeService, _workingDayService);
    }

    #region GetTimeClocksByEmployeeIdAndDateAsync

    [Fact]
    public async Task GetTimeClocksByEmployeeIdAndDateAsync_ShouldReturnsFalseEmptyListError_WithInvalidEmployeeId()
    {
        // Arrange
        var employeeId = Guid.Empty;
        var date = DateTime.Now;

        // Act
        var result = await _timeClockService.GetTimeClocksByEmployeeIdAndDateAsync(employeeId, date);

        // Assert
        Assert.Empty(result);
        Assert.Single(_timeClockService.Notifications);
        Assert.Equal(Error.TimeClock.INVALID_EMPLOYEE_ID, _timeClockService.Notifications.First().Message);
        Assert.Equal("TimeClockService.EmployeeId", _timeClockService.Notifications.First().Key);
    }

    [Fact]
    public async Task GetTimeClocksByEmployeeIdAndDateAsync_ShouldReturnsEmptyList_WithNoData()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var date = DateTime.Now;

        _timeClockRepositoryMock
            .Setup(x => x.GetTimeClocksByEmployeeIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<TimeClock>());

        // Act
        var result = await _timeClockService.GetTimeClocksByEmployeeIdAndDateAsync(employeeId, date);

        // Assert
        Assert.Empty(result);
        Assert.IsAssignableFrom<ICollection<TimeClockDTO>>(result);
        Assert.Empty(_timeClockService.Notifications);
    }

    [Fact]
    public async Task GetTimeClocksByEmployeeIdAndDateAsync_ShouldReturnsList_WithData()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var date = DateTime.Now;

        _timeClockRepositoryMock
            .Setup(x => x.GetTimeClocksByEmployeeIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()))
            .ReturnsAsync(Mocks.GetTimeClocks());

        // Act
        var result = await _timeClockService.GetTimeClocksByEmployeeIdAndDateAsync(employeeId, date);

        // Assert
        Assert.NotEmpty(result);
        Assert.Empty(_timeClockService.Notifications);
        Assert.Equal(Mocks.GetTimeClocks()[0].RegisterType, result.ToList()[0].RegisterType);
        Assert.Equal(Mocks.GetTimeClocks()[0].ClockTimeStatus, result.ToList()[0].ClockTimeStatus);
    }

    #endregion

    #region AddTimeClockAsync
    [Fact]
    public async Task AddTimeClockAsync_ShouldReturnsFalseWithError_WithInvalidEmployeeId()
    {
        // Arrange
        var employeeId = Guid.Empty;
        var registerType = RegisterType.START_WORKING_DAY;
        var model = new TimeClockModel(employeeId, registerType);
        
        // Act
        var result = await _timeClockService.AddTimeClockAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_timeClockService.Notifications);
        Assert.Equal(Error.TimeClock.INVALID_EMPLOYEE_ID, _timeClockService.Notifications.First().Message);
    }
    

    [Theory]
    [InlineData(RegisterType.START_WORKING_DAY)]
    [InlineData(RegisterType.END_WORKING_DAY)]
    [InlineData(RegisterType.START_BREAK)]
    [InlineData(RegisterType.END_BREAK)]
    public async Task AddTimeClockAsync_ShouldReturnsFalseWithError_WithExistentRegisterTypeAtDate(RegisterType registerType)
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var model = new TimeClockModel(employeeId, registerType);
        _timeClockRepositoryMock
            .Setup(x => x.GetTimeClocksByEmployeeIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<TimeClock>
            {
                new (employeeId, registerType)
            });
        
        // Act
        var result = await _timeClockService.AddTimeClockAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_timeClockService.Notifications);
        Assert.Equal(Error.TimeClock.INVALID_REGISTER_TIME, _timeClockService.Notifications.First().Message);
        Assert.Equal("TimeClockService.RegisterTime", _timeClockService.Notifications.First().Key);
    }

    // [Theory]
    // [InlineData(RegisterType.START_WORKING_DAY)]
    // [InlineData(RegisterType.START_BREAK)]
    // [InlineData(RegisterType.END_BREAK)]
    // [InlineData(RegisterType.END_WORKING_DAY)]
    // public async Task AddTimeClockAsync_ShouldReturnsFalseWithError_WithErrorOnSetStatus(RegisterType type)
    // {
    //     // Arrange
    //     var employeeId = Guid.NewGuid();
    //     var registerType = type;
    //     var model = new TimeClockModel(employeeId, registerType);
    //
    //     _employeeRepositoryMock
    //         .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
    //         .ReturnsAsync((Employee?)null);
    //
    //     _workingDayRepositoryMock
    //         .Setup(repo => repo.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
    //         .ReturnsAsync((WorkingDay?)null);
    //
    //     _timeClockRepositoryMock
    //         .Setup(x => x.GetTimeClocksByEmployeeIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()))
    //         .ReturnsAsync(new List<TimeClock>());
    //
    //     var timeClock = new TimeClock(employeeId, registerType);
    //      await _timeClockService.SetClockTimeStatusOnAdd(timeClock, model.EmployeeId);
    //     _timeClockRepositoryMock
    //         .Setup(repo => repo.AddTimeClockAsync(It.IsAny<TimeClock>()))
    //         .ReturnsAsync(false);
    //
    //     // Act
    //     var result = await _timeClockService.AddTimeClockAsync(model);
    //
    //     // Assert
    //     Assert.False(result);
    //     Assert.True(model.IsValid);
    //     Assert.Single(_timeClockService.Notifications);
    //     Assert.Equal(Error.TimeClock.ERROR_SET_STATUS, _timeClockService.Notifications.First().Message);
    //     Assert.Equal("TimeClockService", _timeClockService.Notifications.First().Key);
    // }


    // [Theory]
    // [InlineData(RegisterType.START_WORKING_DAY)]
    // [InlineData(RegisterType.START_BREAK)]
    // [InlineData(RegisterType.END_BREAK)]
    // [InlineData(RegisterType.END_WORKING_DAY)]
    // public async Task AddTimeClockAsync_ShouldReturnsFalseWithError_WithRepositoryError(RegisterType type)
    // {
    //     // Arrange
    //     var employeeId = Guid.NewGuid();
    //     var registerType = type;
    //     var model = new TimeClockModel(employeeId, registerType);
    //
    //     _employeeRepositoryMock
    //         .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
    //         .ReturnsAsync(Mock.Mocks.GetEmployee());
    //
    //     _workingDayRepositoryMock
    //         .Setup(repo => repo.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
    //         .ReturnsAsync(Mock.Mocks.GetWorkingDay());
    //
    //     _timeClockRepositoryMock
    //         .Setup(x => x.GetTimeClocksByEmployeeIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()))
    //         .ReturnsAsync(new List<TimeClock>());
    //
    //     var timeClock = new TimeClock(employeeId, registerType);
    //     await _timeClockService.SetClockTimeStatusOnAdd(timeClock, model.EmployeeId);
    //     _timeClockRepositoryMock
    //         .Setup(repo => repo.AddTimeClockAsync(It.IsAny<TimeClock>()))
    //         .ReturnsAsync(false);
    //
    //
    //     // Act
    //     var result = await _timeClockService.AddTimeClockAsync(model);
    //
    //     // Assert
    //     Assert.False(result);
    //     Assert.True(model.IsValid);
    //     Assert.Single(_timeClockService.Notifications);
    //     Assert.Equal(Error.TimeClock.ADD_TIME_CLOCK_ERROR, _timeClockService.Notifications.First().Message);
    //     Assert.Equal("TimeClockService", _timeClockService.Notifications.First().Key);
    // }
    //
    // [Theory]
    // [InlineData(RegisterType.START_WORKING_DAY)]
    // [InlineData(RegisterType.START_BREAK)]
    // [InlineData(RegisterType.END_BREAK)]
    // [InlineData(RegisterType.END_WORKING_DAY)]
    // public async Task AddTimeClockAsync_ShouldReturnsTrueWithStatusPending_WithIrregularRegisterTime(RegisterType type)
    // {
    //     // Arrange
    //     var employeeId = Guid.NewGuid();
    //     var registerType = type;
    //     var model = new TimeClockModel(employeeId, registerType);
    //
    //     _employeeRepositoryMock            
    //         .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
    //         .ReturnsAsync(Mock.Mocks.GetEmployee());
    //
    //     _workingDayRepositoryMock
    //         .Setup(repo => repo.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
    //         .ReturnsAsync(Mock.Mocks.GetWorkingDay());
    //    
    //     _timeClockRepositoryMock
    //         .Setup(x => x.GetTimeClocksByEmployeeIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()))
    //         .ReturnsAsync(new List<TimeClock>());
    //
    //     var timeClock = new TimeClock(employeeId, registerType);
    //     await _timeClockService.SetClockTimeStatusOnAdd(timeClock, model.EmployeeId);
    //     _timeClockRepositoryMock
    //         .Setup(repo => repo.AddTimeClockAsync(It.IsAny<TimeClock>()))
    //         .ReturnsAsync(true);
    //
    //     
    //     // Act
    //     var result = await _timeClockService.AddTimeClockAsync(model);
    //     
    //     // Assert
    //     Assert.True(result);
    //     Assert.True(model.IsValid);
    //     Assert.Empty(_timeClockService.Notifications);
    //     Assert.Equal(ClockTimeStatus.PENDING, timeClock.ClockTimeStatus);
    // }

    #endregion

    #region UpdateTimeClockStatus

    [Fact]
    public async Task UpdateTimeClockStatus_ShouldReturnFalse_WithInvalidTimeClockId()
    {
        // Arrange
        var timeClockId = Guid.Empty;
        var status = ClockTimeStatus.APPROVED;

        // Act
        var result = await _timeClockService.UpdateTimeClockStatus(timeClockId, status);

        // Assert
        Assert.False(result);
        Assert.Single(_timeClockService.Notifications);
        Assert.Equal(Error.TimeClock.INVALID_TIME_CLOCK_ID, _timeClockService.Notifications.First().Message);
        Assert.Equal("TimeClockService.TimeClockId", _timeClockService.Notifications.First().Key);
    }

    [Fact]
    public async Task UpdateTimeClockStatus_ShouldReturnsFalseWithError_WithNotFoundedTimeClock()
    {
        // Arrange
        var timeClockId = Guid.NewGuid();
        var status = ClockTimeStatus.APPROVED;

        _timeClockRepositoryMock
            .Setup(x => x.GetTimeClockByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((TimeClock?)null);

        // Act
        var result = await _timeClockService.UpdateTimeClockStatus(timeClockId, status);

        // Assert
        Assert.False(result);
        Assert.Single(_timeClockService.Notifications);
        Assert.Equal(Error.TimeClock.TIME_CLOCK_NOT_FOUND, _timeClockService.Notifications.First().Message);
        Assert.Equal("TimeClockService.TimeClockId", _timeClockService.Notifications.First().Key);
    }

    [Fact]
    public async Task UpdateTimeClockStatus_ShouldReturnsFalseWithError_WithRepositoryError()
    {
        // Arrange
        var timeClockId = Guid.NewGuid();
        var status = ClockTimeStatus.APPROVED;

        _timeClockRepositoryMock
            .Setup(x => x.GetTimeClockByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Mocks.GetTimeClock());

        _timeClockRepositoryMock
            .Setup(x => x.UpdateTimeClockAsync(It.IsAny<TimeClock>()))
            .ReturnsAsync(false);

        // Act
        var result = await _timeClockService.UpdateTimeClockStatus(timeClockId, status);

        // Assert
        Assert.False(result);
        Assert.Single(_timeClockService.Notifications);
        Assert.Equal(Error.TimeClock.UPDATE_TIME_CLOCK_ERROR, _timeClockService.Notifications.First().Message);
        Assert.Equal("TimeClockService", _timeClockService.Notifications.First().Key);
    }

    [Fact]
    public async Task UpdateTimeClockStatus_ShouldReturnsTrue_WithSuccess()
    {
        // Arrange
        var timeClockId = Guid.NewGuid();
        var status = ClockTimeStatus.REJECTED;

        _timeClockRepositoryMock
            .Setup(x => x.GetTimeClockByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Mocks.GetTimeClock());

        _timeClockRepositoryMock
            .Setup(x => x.UpdateTimeClockAsync(It.IsAny<TimeClock>()))
            .ReturnsAsync(true);

        // Act
        var result = await _timeClockService.UpdateTimeClockStatus(timeClockId, status);

        // Assert
        Assert.True(result);
        Assert.Empty(_timeClockService.Notifications);
    }

    #endregion

    // #region SetClockTimeStatusOnAdd
    // [Fact]
    // public async Task SetClockTimeStatusOnAdd_ShouldReturn_WithInvalidEmployeeId()
    // {
    //     // Arrange
    //     var employeeId = Guid.Empty;
    //     var registerType = RegisterType.START_WORKING_DAY;
    //     var timeClock = new TimeClock(employeeId, registerType);
    //
    //     // Act
    //     await _timeClockService.SetClockTimeStatusOnAdd(timeClock, employeeId);
    //
    //     // Assert
    //     Assert.Equal(ClockTimeStatus.APPROVED, timeClock.ClockTimeStatus);
    // }
    //
    // [Fact]
    // public async Task SetClockTimeStatusOnAdd_ShouldReturn_WithInvalidWorkingDay()
    // {
    //     // Arrange
    //     var employeeId = Guid.NewGuid();
    //     var registerType = RegisterType.START_WORKING_DAY;
    //     var timeClock = new TimeClock(employeeId, registerType);
    //
    //     _workingDayRepositoryMock
    //         .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
    //         .ReturnsAsync((WorkingDay?)null);
    //
    //     // Act
    //     await _timeClockService.SetClockTimeStatusOnAdd(timeClock, employeeId);
    //
    //     // Assert
    //     Assert.Equal(ClockTimeStatus.APPROVED, timeClock.ClockTimeStatus);
    // }
    //
    // [Theory]
    // [InlineData(RegisterType.START_WORKING_DAY)]
    // [InlineData(RegisterType.START_BREAK)]
    // [InlineData(RegisterType.END_BREAK)]
    // [InlineData(RegisterType.END_WORKING_DAY)]
    // public async Task SetClockTimeStatusOnAdd_ShouldCompleteSetClockTimeStatus_WithWorkingDay(RegisterType type)
    // {
    //     // Arrange
    //     var employeeId = Guid.NewGuid();
    //     var timeClock = new TimeClock(employeeId, type);
    //
    //     _employeeRepositoryMock
    //         .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
    //         .ReturnsAsync(Mock.Mocks.GetEmployee());
    //
    //     _workingDayRepositoryMock
    //         .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
    //         .ReturnsAsync(Mock.Mocks.GetWorkingDay());
    //
    //     // Act
    //     await _timeClockService.SetClockTimeStatusOnAdd(timeClock, employeeId);
    //
    //     // Assert
    //     Assert.Equal(ClockTimeStatus.PENDING, timeClock.ClockTimeStatus);
    // }
    //
    // #endregion
}