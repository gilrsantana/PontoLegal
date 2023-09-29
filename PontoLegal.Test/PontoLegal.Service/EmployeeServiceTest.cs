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

    #region GetEmployeeByIdAsync   
    
    [Fact]
    public async Task GetEmployeeByIdAsync_ShouldReturnsNullWithError_WithUnknownEmployee()
    {
        // Arrange
        var id = Guid.NewGuid();
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Employee?)null);
        
        // Act
        var result = await _employeeService.GetEmployeeByIdAsync(id);

        // Assert
        Assert.Null(result);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.EMPLOYEE_NOT_FOUNDED, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.Id", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task GetEmployeeByIdAsync_ShouldReturnsNullWithError_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;
        
        // Act
        var result = await _employeeService.GetEmployeeByIdAsync(id);

        // Assert
        Assert.Null(result);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.INVALID_ID, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.Id", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task GetEmployeeByIdAsync_ShouldReturnsEmployeeDTO()
    {
        // Arrange
        var id = Guid.NewGuid();
        var employee = MockEmployee.GetEmployee();
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(employee);
        
        // Act
        var result = await _employeeService.GetEmployeeByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<EmployeeDTO?>(result);
        Assert.Empty(_employeeService.Notifications);
        Assert.Equal(employee.Id, result.EmployeeId);
        Assert.Equal(employee.Name, result.Name);
        Assert.Equal(employee.HireDate, result.HireDate);
        Assert.Equal(employee.RegistrationNumber, result.RegistrationNumber);
        Assert.Equal(employee.JobPositionId, result.JobPositionId);
        Assert.Equal(employee.Pis.Number, result.PisNumber);
        Assert.Equal(employee.CompanyId, result.CompanyId);
        Assert.Equal(employee.WorkingDayId, result.WorkingDayId);
        Assert.Equal(employee.ManagerId, result.ManagerId);
    }
    #endregion
    
    #region GetEmployeeByPisAsync
    
    [Theory]
    [InlineData("")]
    [InlineData("1234567890")]
    [InlineData("654.18058.84-3")]
    [InlineData("123456789012")]
    
    public async Task GetEmployeeByPisAsync_ShouldReturnsNullWithError_WithInvalidPisLength(string numberPis)
    {
        // Arrange

        
        // Act
        var result = await _employeeService.GetEmployeeByPisAsync(numberPis);

        // Assert
        Assert.Null(result);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Pis.INVALID_PIS_FORMAT, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.Pis", _employeeService.Notifications.First().Key);
    }
    
    [Theory]
    [InlineData("12345678901")]
    [InlineData("35463463700")]
    [InlineData("13445709340")]
    public async Task GetEmployeeByPisAsync_ShouldReturnsNullWithError_WithInvalidPisDigits(string numberPis)
    {
        // Arrange

        
        // Act
        var result = await _employeeService.GetEmployeeByPisAsync(numberPis);

        // Assert
        Assert.Null(result);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Pis.INVALID_PIS_DIGITS, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.Pis", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task GetEmployeeByPisAsync_ShouldReturnsNullWithError_WithUnknownEmployee()
    {
        // Arrange
        var pis = new Pis(MockPis.ValidPis);
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByPisAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee?)null);
        
        // Act
        var result = await _employeeService.GetEmployeeByPisAsync(pis.Number);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetEmployeeByPisAsync_ShouldReturnsEmployeeDTO()
    {
        // Arrange
        var pis = new Pis(MockPis.ValidPis);
        var employee = MockEmployee.GetEmployee();
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByPisAsync(It.IsAny<string>()))
            .ReturnsAsync(employee);
        
        // Act
        var result = await _employeeService.GetEmployeeByPisAsync(pis.Number);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<EmployeeDTO?>(result);
        Assert.Empty(_employeeService.Notifications);
        Assert.Equal(employee.Id, result.EmployeeId);
        Assert.Equal(employee.Name, result.Name);
        Assert.Equal(employee.HireDate, result.HireDate);
        Assert.Equal(employee.RegistrationNumber, result.RegistrationNumber);
        Assert.Equal(employee.JobPositionId, result.JobPositionId);
        Assert.Equal(employee.Pis.Number, result.PisNumber);
        Assert.Equal(employee.CompanyId, result.CompanyId);
        Assert.Equal(employee.WorkingDayId, result.WorkingDayId);
        Assert.Equal(employee.ManagerId, result.ManagerId);
    }
    #endregion
    
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
        var pis = MockPis.ValidPis;
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
        Assert.Equal("EmployeeModel.Name", model.Notifications.First().Key);
        Assert.Equal("EmployeeModel.Name", _employeeService.Notifications.First().Key);
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
        var pis = MockPis.ValidPis;
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
        Assert.Equal("EmployeeModel.RegistrationNumber", model.Notifications.First().Key);
        Assert.Equal("EmployeeModel.RegistrationNumber", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidHireDate()
    {
        // Arrange
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var registrationNumber = "123456789";
        var jobPositionId = Guid.NewGuid();
        var pis = MockPis.ValidPis;
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
        Assert.Equal("EmployeeModel.HireDate", model.Notifications.First().Key);
        Assert.Equal("EmployeeModel.HireDate", _employeeService.Notifications.First().Key);
    }

    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidJobPositionId()
    {
        // Arrange
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now);
        var registrationNumber = "123456789";
        var jobPositionId = Guid.Empty;
        var pis = MockPis.ValidPis;
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
        Assert.Equal("EmployeeModel.JobPositionId", model.Notifications.First().Key);
        Assert.Equal("EmployeeModel.JobPositionId", _employeeService.Notifications.First().Key);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    [InlineData("61423652148")]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidPis(string pisNumber)
    {
        // Arrange
        var model = MockEmployee.GetEmployeeModel();
        model.Pis = pisNumber;
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
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_employeeService.Notifications);
        Assert.Contains(_employeeService.Notifications, x => x.Message == Error.Pis.INVALID_PIS_DIGITS || x.Message == Error.Pis.INVALID_PIS_FORMAT);
        Assert.Contains(_employeeService.Notifications, x => x.Key == "EmployeeService.Pis");
    }

    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidCompanyId()
    {
        // Arrange
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now);
        var registrationNumber = "123456789";
        var jobPositionId = Guid.NewGuid();
        var pis = MockPis.ValidPis;
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
        Assert.Equal("EmployeeModel.CompanyId", model.Notifications.First().Key);
        Assert.Equal("EmployeeModel.CompanyId", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidWorkingDayId()
    {
        // Arrange
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now);
        var registrationNumber = "123456789";
        var jobPositionId = Guid.NewGuid();
        var pis = MockPis.ValidPis;
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
        Assert.Equal("EmployeeModel.WorkingDayId", model.Notifications.First().Key);
        Assert.Equal("EmployeeModel.WorkingDayId", _employeeService.Notifications.First().Key);
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
        Assert.Equal("EmployeeService.JobPositionId", _employeeService.Notifications.First().Key);
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
        Assert.Equal("EmployeeService.CompanyId", _employeeService.Notifications.First().Key);
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
        Assert.Equal("EmployeeService.WorkingDayId", _employeeService.Notifications.First().Key);
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
        Assert.Equal("EmployeeService.Pis", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnsFalseWithError_WithErrorOnAdding()
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
            .ReturnsAsync(false);
        
        // Act
        var result = await _employeeService.AddEmployeeAsync(model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.ERROR_ADDING, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService", _employeeService.Notifications.First().Key);
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
    
    #region UpdateEmployeeAsync
    
    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;
        var model = MockEmployee.GetEmployeeModel();
        
        // Act
        var result = await _employeeService.UpdateEmployeeAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.INVALID_ID, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.Id", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnsFalseWithError_WithUnknownEmployee()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = MockEmployee.GetEmployeeModel();
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Employee?)null);
        
        // Act
        var result = await _employeeService.UpdateEmployeeAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.EMPLOYEE_NOT_FOUNDED, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.Id", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnsFalseWithError_WithInvalidModel()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "";
        var hireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var registrationNumber = "";
        var jobPositionId = Guid.Empty;
        var pis = "";
        var companyId = Guid.Empty;
        var managerId = Guid.Empty;
        var workingDayId = Guid.Empty;
        var model = new EmployeeModel(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockEmployee.GetEmployee());
        // Act
        var result = await _employeeService.UpdateEmployeeAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.Notifications.Count == 7);
        Assert.True(_employeeService.Notifications.Count == 7);
        Assert.Contains(model.Notifications, x => x.Key == "EmployeeModel.Name");
        Assert.Contains(model.Notifications, x => x.Key == "EmployeeModel.HireDate");
        Assert.Contains(model.Notifications, x => x.Key == "EmployeeModel.RegistrationNumber");
        Assert.Contains(model.Notifications, x => x.Key == "EmployeeModel.JobPositionId");
        Assert.Contains(model.Notifications, x => x.Key == "EmployeeModel.Pis");
        Assert.Contains(model.Notifications, x => x.Key == "EmployeeModel.CompanyId");
        Assert.Contains(model.Notifications, x => x.Key == "EmployeeModel.WorkingDayId");
    }
    
    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnsFalseWithError_WithUnknownJobPositionId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = MockEmployee.GetEmployeeModel();
        
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((JobPosition?)null);
        
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockEmployee.GetEmployee());
        
        // Act
        var result = await _employeeService.UpdateEmployeeAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.JOB_POSITION_NOT_FOUNDED, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.JobPositionId", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnsFalseWithError_WithUnknownCompanyId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = MockEmployee.GetEmployeeModel();
        
        _jobPositionRepositoryMock
            .Setup(x => x.GetJobPositionByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new JobPosition("Job Position", Guid.NewGuid(), new Department("Department")));
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Company?)null);
        
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockEmployee.GetEmployee());
        
        // Act
        var result = await _employeeService.UpdateEmployeeAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.COMPANY_NOT_FOUNDED, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.CompanyId", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnsFalseWithError_WithUnknownWorkingDayId()
    {
        // Arrange
        var id = Guid.NewGuid();
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
        
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockEmployee.GetEmployee());
        
        // Act
        var result = await _employeeService.UpdateEmployeeAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.WORKING_DAY_NOT_FOUNDED, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.WorkingDayId", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnsFalseWithError_WithDuplicatedEmployee()
    {
        // Arrange
        var id = Guid.NewGuid();
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
        
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockEmployee.GetEmployee());
        
        // Act
        var result = await _employeeService.UpdateEmployeeAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.PIS_ALREADY_EXISTS, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.Pis", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnsFalseWithError_WithErrorOnUpdating()
    {
        // Arrange
        var id = Guid.NewGuid();
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
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockEmployee.GetEmployee());
        
        _employeeRepositoryMock
            .Setup(x => x.UpdateEmployeeAsync(It.IsAny<Guid>(), It.IsAny<Employee>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _employeeService.UpdateEmployeeAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.ERROR_UPDATING, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
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
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockEmployee.GetEmployee());
        
        _employeeRepositoryMock
            .Setup(x => x.UpdateEmployeeAsync(It.IsAny<Guid>(), It.IsAny<Employee>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _employeeService.UpdateEmployeeAsync(id, model);

        // Assert
        Assert.True(result);
        Assert.True(model.IsValid);
        Assert.Empty(_employeeService.Notifications);
    }
    
    #endregion
    
    #region RemoveEmployeeByIdAsync
    
    [Fact]
    public async Task RemoveEmployeeByIdAsync_ShouldReturnsFalseWithError_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;
        
        // Act
        var result = await _employeeService.RemoveEmployeeByIdAsync(id);

        // Assert
        Assert.False(result);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.INVALID_ID, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.Id", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task RemoveEmployeeByIdAsync_ShouldReturnsFalseWithError_WithUnknownEmployee()
    {
        // Arrange
        var id = Guid.NewGuid();
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Employee?)null);
        
        // Act
        var result = await _employeeService.RemoveEmployeeByIdAsync(id);

        // Assert
        Assert.False(result);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.EMPLOYEE_NOT_FOUNDED, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService.Id", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task RemoveEmployeeByIdAsync_ShouldReturnsFalseWithError_WithErrorOnRemoving()
    {
        // Arrange
        var id = Guid.NewGuid();
        var employee = MockEmployee.GetEmployee();
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(employee);
        
        _employeeRepositoryMock
            .Setup(x => x.RemoveEmployeeByIdAsync(It.IsAny<Employee>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _employeeService.RemoveEmployeeByIdAsync(id);

        // Assert
        Assert.False(result);
        Assert.Single(_employeeService.Notifications);
        Assert.Equal(Error.Employee.ERROR_REMOVING, _employeeService.Notifications.First().Message);
        Assert.Equal("EmployeeService", _employeeService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task RemoveEmployeeByIdAsync_ShouldReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var employee = MockEmployee.GetEmployee();
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(employee);
        
        _employeeRepositoryMock
            .Setup(x => x.RemoveEmployeeByIdAsync(It.IsAny<Employee>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _employeeService.RemoveEmployeeByIdAsync(id);

        // Assert
        Assert.True(result);
        Assert.Empty(_employeeService.Notifications);
    }
    
    #endregion
}