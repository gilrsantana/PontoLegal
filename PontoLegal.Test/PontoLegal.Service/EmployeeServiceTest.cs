using PontoLegal.Service.Interfaces;

namespace PontoLegal.Test.PontoLegal.Service;

public class EmployeeServiceTest
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly EmployeeService _employeeService;
    private readonly IJobPositionService _jobPositionServiceMock;
    private readonly Mock<IJobPositionRepository> _jobPositionRepositoryMock;
    private readonly ICompanyService _companyServiceMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly IWorkingDayService _workingDayServiceMock;
    private readonly Mock<IWorkingDayRepository> _workingDayRepositoryMock;
    
    public EmployeeServiceTest()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _jobPositionRepositoryMock = new Mock<IJobPositionRepository>();
        _jobPositionServiceMock = new JobPositionService(_jobPositionRepositoryMock.Object);
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _companyServiceMock = new CompanyService(_companyRepositoryMock.Object);
        _workingDayRepositoryMock = new Mock<IWorkingDayRepository>();
        _workingDayServiceMock = new WorkingDayService(_workingDayRepositoryMock.Object);
        _employeeService = new EmployeeService(_employeeRepositoryMock.Object, _jobPositionServiceMock, _companyServiceMock, _workingDayServiceMock);
    }

    #region AddEmployeeAsync
    
    [Theory]
    [InlineData("")]
    [InlineData("E")]
    [InlineData("Em")]
    [InlineData("This is a name of employee that has more than 80 characters in order to not validate")]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidName(string name)
    {
        // Arrange
        var hireDate = DateOnly.FromDateTime(DateTime.Now);
        var registrationNumber = "123456789";
        var jobPositionId = Guid.NewGuid();
        var pis = new Pis(MockPis.ValidPis);
        var companyId = Guid.NewGuid();
        var managerId = Guid.Empty;
        var workingDayId = Guid.NewGuid();
        var model = new EmployeeModel(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.Single(model.Notifications);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.INVALID_NAME, model.Notifications.First().Message);
        Assert.Equal(Error.Employee.INVALID_NAME, _employeeService.Notifications.First().Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123456789012345678901")]
    [InlineData("a1s2d3f4g5h6j7k8l9p0z")]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidRegistrationNumber(string reg)
    {
        // Arrange
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now);
        var registrationNumber = reg;
        var jobPositionId = Guid.NewGuid();
        var pis = new Pis(MockPis.ValidPis);
        var companyId = Guid.NewGuid();
        var managerId = Guid.Empty;
        var workingDayId = Guid.NewGuid();
        var model = new EmployeeModel(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.Single(model.Notifications);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.INVALID_REGISTRATION_NUMBER, model.Notifications.First().Message);
        Assert.Equal(Error.Employee.INVALID_REGISTRATION_NUMBER, _employeeService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidHireDate()
    {
        // Arrange
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var registrationNumber = "123456789";
        var jobPositionId = Guid.NewGuid();
        var pis = new Pis(MockPis.ValidPis);
        var companyId = Guid.NewGuid();
        var managerId = Guid.Empty;
        var workingDayId = Guid.NewGuid();
        var model = new EmployeeModel(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.Single(model.Notifications);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.INVALID_HIRE_DATE, model.Notifications.First().Message);
        Assert.Equal(Error.Employee.INVALID_HIRE_DATE, _employeeService.Notifications.First().Message);
    }

    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidJobPositionId()
    {
        // Arrange
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now);
        var registrationNumber = "123456789";
        var jobPositionId = Guid.Empty;
        var pis = new Pis(MockPis.ValidPis);
        var companyId = Guid.NewGuid();
        var managerId = Guid.Empty;
        var workingDayId = Guid.NewGuid();
        var model = new EmployeeModel(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.Single(model.Notifications);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.INVALID_JOB_POSITION_ID, model.Notifications.First().Message);
        Assert.Equal(Error.Employee.INVALID_JOB_POSITION_ID, _employeeService.Notifications.First().Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    [InlineData("61423652148")]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidPis(string pisNumber)
    {
        // Arrange
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now);
        var registrationNumber = "123456789";
        var jobPositionId = Guid.NewGuid();
        var pis = new Pis(pisNumber);
        var companyId = Guid.NewGuid();
        var managerId = Guid.Empty;
        var workingDayId = Guid.NewGuid();
        var model = new EmployeeModel(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.Single(model.Notifications);
        Assert.Single(_employeeService.Notifications);
        Assert.Contains(model.Notifications, x => x.Message == Error.Pis.INVALID_PIS_DIGITS || x.Message == Error.Pis.INVALID_PIS_FORMAT);
        Assert.Contains(_employeeService.Notifications, x => x.Message == Error.Pis.INVALID_PIS_DIGITS || x.Message == Error.Pis.INVALID_PIS_FORMAT);
    }

    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidCompanyId()
    {
        // Arrange
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now);
        var registrationNumber = "123456789";
        var jobPositionId = Guid.NewGuid();
        var pis = new Pis(MockPis.ValidPis);
        var companyId = Guid.Empty;
        var managerId = Guid.Empty;
        var workingDayId = Guid.NewGuid();
        var model = new EmployeeModel(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.Single(model.Notifications);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.INVALID_COMPANY_ID, model.Notifications.First().Message);
        Assert.Equal(Error.Employee.INVALID_COMPANY_ID, _employeeService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidWorkingDayId()
    {
        // Arrange
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now);
        var registrationNumber = "123456789";
        var jobPositionId = Guid.NewGuid();
        var pis = new Pis(MockPis.ValidPis);
        var companyId = Guid.NewGuid();
        var managerId = Guid.Empty;
        var workingDayId = Guid.Empty;
        var model = new EmployeeModel(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.Single(model.Notifications);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.INVALID_WORKING_DAY_ID, model.Notifications.First().Message);
        Assert.Equal(Error.Employee.INVALID_WORKING_DAY_ID, _employeeService.Notifications.First().Message);
    }

    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithUnknownJobPositionId()
    {
        // Arrange
        var model = MockEmployee.GetEmployeeModel();
        
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((JobPosition?)null);
        
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.JOB_POSITION_NOT_FOUNDED, _employeeService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithUnknownCompanyId()
    {
        // Arrange
        var model = MockEmployee.GetEmployeeModel();
        
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new JobPosition("Job Position", Guid.NewGuid(), new Department("Department")));
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Company?)null);
        
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.COMPANY_NOT_FOUNDED, _employeeService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithUnknownWorkingDayId()
    {
        // Arrange
        var model = MockEmployee.GetEmployeeModel();
        
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new JobPosition("Job Position", Guid.NewGuid(), new Department("Department")));
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Company("Company", new Cnpj(MockCnpj.ValidCnpj)));
        
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((WorkingDay?)null);
        
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.WORKING_DAY_NOT_FOUNDED, _employeeService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithDuplicatedEmployee()
    {
        // Arrange
        var model = MockEmployee.GetEmployeeModel();
        var workingDay = new WorkingDay(
            "Working Day",
            WorkingDayType.TEN_HOURS,
            new TimeOnly(8, 0),
            new TimeOnly(12, 0),
            new TimeOnly(14, 0),
            new TimeOnly(18, 0)
        );
        var employee = MockEmployee.GetEmployee();
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new JobPosition("Job Position", Guid.NewGuid(), new Department("Department")));
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Company("Company", new Cnpj(MockCnpj.ValidCnpj)));
        
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(workingDay);
        
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByPisAsync(It.IsAny<string>()))
            .ReturnsAsync(employee);
        
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.PIS_ALREADY_EXISTS, _employeeService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsTrue()
    {
        // Arrange
        var model = MockEmployee.GetEmployeeModel();
        var workingDay = new WorkingDay(
            "Working Day",
            WorkingDayType.TEN_HOURS,
            new TimeOnly(8, 0),
            new TimeOnly(12, 0),
            new TimeOnly(14, 0),
            new TimeOnly(18, 0)
        );
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new JobPosition("Job Position", Guid.NewGuid(), new Department("Department")));
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Company("Company", new Cnpj(MockCnpj.ValidCnpj)));
        
        _workingDayRepositoryMock
            .Setup(x => x.GetWorkingDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(workingDay);
        
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByPisAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee?)null);
        
        _employeeRepositoryMock
            .Setup(x => x.AddEmployeeAsync(It.IsAny<Employee>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.True(result);
        Assert.True(model.IsValid);
        Assert.Empty(_employeeService.Notifications);
    }
    #endregion
}