namespace PontoLegal.Test.PontoLegal.Service;

public class TimeClockServiceTest
{
    private readonly Mock<ITimeClockRepository> _timeClockRepositoryMock;
    private readonly TimeClockService _timeClockService;

    public TimeClockServiceTest()
    {
        _timeClockRepositoryMock = new Mock<ITimeClockRepository>();
        _timeClockService = new TimeClockService(_timeClockRepositoryMock.Object);
    }

    [Fact]
    public async Task AddTimeClockAsync_ShouldReturnsFalseWithError_WithInvalidEmployeeId()
    {
        // Arrange
        var register = DateTime.Now;
        var employeeId = Guid.Empty;
        var registerType = RegisterType.START_WORKING_DAY;
        var model = new TimeClockModel(register, employeeId, registerType);
        
        // Act
        var result = await _timeClockService.AddTimeClockAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_timeClockService.Notifications);
        Assert.Equal(Error.TimeClock.INVALID_EMPLOYEE_ID, _timeClockService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddTimeClockAsync_ShouldReturnsFalseWithError_WithInvalidRegisterTime()
    {
        // Arrange
        var register = DateTime.Now.AddDays(1);
        var employeeId = Guid.NewGuid();
        var registerType = RegisterType.START_WORKING_DAY;
        var model = new TimeClockModel(register, employeeId, registerType);
        
        // Act
        var result = await _timeClockService.AddTimeClockAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.Single(model.Notifications);
        Assert.Single(_timeClockService.Notifications);
        Assert.Equal(Error.TimeClock.INVALID_REGISTER_TIME, _timeClockService.Notifications.First().Message);
    }

    [Theory]
    [InlineData(RegisterType.START_WORKING_DAY)]
    [InlineData(RegisterType.END_WORKING_DAY)]
    [InlineData(RegisterType.START_BREAK)]
    [InlineData(RegisterType.END_BREAK)]
    public async Task AddTimeClockAsync_ShouldReturnsFalseWithError_WithExistentRegisterTypeAtDate(RegisterType registerType)
    {
        // Arrange
        var register = DateTime.Now;
        var employeeId = Guid.NewGuid();
        var model = new TimeClockModel(register, employeeId, registerType);
        _timeClockRepositoryMock
            .Setup(x => x.GetTimeClocksByEmployeeIdAndDateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<TimeClock>
            {
                new TimeClock(register, employeeId, registerType, ClockTimeStatus.PENDING)
            });
        
        // Act
        var result = await _timeClockService.AddTimeClockAsync(model);
        
        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_timeClockService.Notifications);
        Assert.Equal(Error.TimeClock.INVALID_REGISTER_TIME, _timeClockService.Notifications.First().Message);
    }



}