using PontoLegal.Service.Entities;

namespace PontoLegal.Test.PontoLegal.Service;

public class DepartmentServiceTest
{
    private readonly Mock<IDepartmentRepository> _departmentRepositoryMock;
    private readonly DepartmentService _departmentService;

    public DepartmentServiceTest()
    {
        _departmentRepositoryMock = new Mock<IDepartmentRepository>();
        _departmentService = new DepartmentService(_departmentRepositoryMock.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData("D")]
    [InlineData("Products Development Engineering Department")]
    public async Task AddDepartmentAsync_ShouldReturnFalseWithErrors_WithInvalidName(string name)
    {
        // Arrange
        var department = new Department(name);
        var model = new DepartmentModel(name);
        _departmentRepositoryMock.Setup(repo => repo.AddDepartmentAsync(department))
            .ReturnsAsync(true);

        // Act
        var result = await _departmentService.AddDepartmentAsync(model);

        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.False(_departmentService.IsValid);
        Assert.Equal(1, _departmentService.Notifications.Count);
        Assert.Equal(Error.Department.INVALID_NAME, _departmentService.Notifications.ElementAt(0).Message);
    }

    [Fact]
    public async Task AddDepartmentAsync_ShouldReturnFalseWithErrors_WithRepeatedName()
    {
        // Arrange
        var department = new Department("Products Development");
        var model = new DepartmentModel("Products Development");
        _departmentRepositoryMock.Setup(repo => repo.AddDepartmentAsync(department))
            .ReturnsAsync(true);
        _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByNameAsync(model.Name))
            .ReturnsAsync(department);

        // Act
        var result = await _departmentService.AddDepartmentAsync(model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.NAME_ALREADY_EXISTS, _departmentService.Notifications.ElementAt(0).Message);
    }

    [Fact]
    public async Task AddDepartmentAsync_ShouldReturnFalseWithErrors_WithErrorInRepository()
    {
        // Arrange
        var department = new Department("Products Development");
        var model = new DepartmentModel("Products Development");
        _departmentRepositoryMock.Setup(repo => repo.AddDepartmentAsync(department))
            .ReturnsAsync(false);
        _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByNameAsync(model.Name))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _departmentService.AddDepartmentAsync(model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.ERROR_ADDING, _departmentService.Notifications.ElementAt(0).Message);
    }

    [Fact]
    public async Task AddDepartmentAsync_ShouldReturnTrue_WithValidName()
    {
        // Arrange
        var department = new Department("Products Development");
        var model = new DepartmentModel("Products Development");
        _departmentRepositoryMock.Setup(repo => repo.AddDepartmentAsync(department))
            .ReturnsAsync(true);
        _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByNameAsync(model.Name))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _departmentService.AddDepartmentAsync(model);

        // Assert
        Assert.True(result);
        Assert.True(model.IsValid);
        Assert.True(_departmentService.IsValid);
        Assert.Empty(_departmentService.Notifications);
    }

    [Theory]
    [InlineData("")]
    [InlineData("D")]
    [InlineData("Products Development Engineering Department")]
    public async Task UpdateDepartmentAsync_ShouldReturnFalse_WithInvalidName(string name)
    {
        // Arrange
        var department = new Department(name);
        var model = new DepartmentModel(name);
        var id = Guid.NewGuid();
        _departmentRepositoryMock.Setup(repo => repo.UpdateDepartmentAsync(id, department))
            .ReturnsAsync(true);

        // Act
        var result = await _departmentService.UpdateDepartmentAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.INVALID_NAME, _departmentService.Notifications.ElementAt(0).Message);
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ShouldReturnFalseWithErrors_WithRepeatedName()
    {
        // Arrange
        var department1 = new Department("Products Development");
        var department2 = new Department("Products Development");
        var model = new DepartmentModel("Products Development");
        var id = Guid.NewGuid();
        _departmentRepositoryMock.Setup(repo => repo.UpdateDepartmentAsync(id, department1))
            .ReturnsAsync(true);
        _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByNameAsync(model.Name))
            .ReturnsAsync(department2);
        _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByIdAsync(department1.Id))
            .ReturnsAsync(department1);

        // Act
        var result = await _departmentService.UpdateDepartmentAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.NAME_ALREADY_EXISTS, _departmentService.Notifications.ElementAt(0).Message);
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ShouldReturnFalseWithErrors_WithIdNotExists()
    {
        // Arrange
        var department = new Department("Products Development");
        var model = new DepartmentModel("Products Development");
        var id = Guid.NewGuid();
        _departmentRepositoryMock.Setup(repo => repo.UpdateDepartmentAsync(department.Id, department))
            .ReturnsAsync(true);
        _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByNameAsync(model.Name))
            .ReturnsAsync((Department?)null);
        _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByIdAsync(id))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _departmentService.UpdateDepartmentAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.DEPARTMENT_NOT_FOUNDED, _departmentService.Notifications.ElementAt(0).Message);
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ShouldReturnFalseWithErrors_WithErrorInRepository()
    {
        // Arrange
        var department = new Department("Products Development");
        var model = new DepartmentModel("Products Development");
        var id = Guid.NewGuid();
        _departmentRepositoryMock.Setup(repo => repo.UpdateDepartmentAsync(id, department))
            .ReturnsAsync(false);
        _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByNameAsync(model.Name))
            .ReturnsAsync((Department?)null);
        _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByIdAsync(department.Id))
            .ReturnsAsync(department);

        // Act
        var result = await _departmentService.UpdateDepartmentAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.ERROR_UPDATING, _departmentService.Notifications.ElementAt(0).Message);
    }

    //[Fact]
    //public async Task UpdateDepartmentAsync_ShouldReturnTrue_WithValidName()
    //{
    //    // Arrange
    //    var Department = new Department("Engenharia de Produtos");
    //    _departmentRepositoryMock.Setup(repo => repo.UpdateDepartmentAsync(Department))
    //        .ReturnsAsync(true);
    //    _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByNomeAsync(Department.Nome))
    //        .ReturnsAsync((Department?)null);
    //    _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByIdAsync(Department.Id))
    //        .ReturnsAsync(Department);

    //    // Act
    //    var result = await _departmentService.UpdateDepartmentAsync(Department);

    //    // Assert
    //    Assert.True(result);
    //    Assert.True(Department.IsValid);
    //    Assert.True(_departmentService.IsValid);
    //    Assert.Empty(_departmentService.Notifications);

    //    _departmentRepositoryMock
    //        .Verify(x => x.UpdateDepartmentAsync(It.IsAny<Department>()), Times.Once);

    //    _departmentRepositoryMock
    //        .Verify(x => x.GetDepartmentByNomeAsync(It.IsAny<string>()), Times.Once);

    //    _departmentRepositoryMock
    //        .Verify(x => x.GetDepartmentByIdAsync(It.IsAny<Guid>()), Times.Once);
    //}

    //[Fact]
    //public async Task RemoveDepartmentAsync_ShouldReturnFalseWithErrors_WithIdNotExists()
    //{
    //    // Arrange
    //    var Department = new Department("Engenharia de Produtos");
    //    _departmentRepositoryMock.Setup(repo => repo.RemoveDepartmentAsync(Department))
    //        .ReturnsAsync(true);
    //    _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByIdAsync(Department.Id))
    //        .ReturnsAsync((Department?)null);

    //    // Act
    //    var result = await _departmentService.RemoveDepartmentAsync(Department);

    //    // Assert
    //    Assert.False(result);
    //    Assert.True(Department.IsValid);
    //    Assert.False(_departmentService.IsValid);
    //    Assert.Single(_departmentService.Notifications);
    //    Assert.Equal(Error.Department.Department_NAO_ENCONTRADO, _departmentService.Notifications.ElementAt(0).Message);

    //    _departmentRepositoryMock
    //        .Verify(x => x.RemoveDepartmentAsync(It.IsAny<Department>()), Times.Never);

    //    _departmentRepositoryMock
    //        .Verify(x => x.GetDepartmentByIdAsync(It.IsAny<Guid>()), Times.Once);
    //}

    //[Fact]
    //public async Task RemoveDepartmentAsync_ShouldReturnReturnFalseWithErrors_WithErrorInRepository()
    //{
    //    // Arrange
    //    var Department = new Department("Engenharia de Produtos");
    //    _departmentRepositoryMock.Setup(repo => repo.RemoveDepartmentAsync(Department))
    //        .ReturnsAsync(false);
    //    _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByIdAsync(Department.Id))
    //        .ReturnsAsync(Department);

    //    // Act
    //    var result = await _departmentService.RemoveDepartmentAsync(Department);

    //    // Assert
    //    Assert.False(result);
    //    Assert.True(Department.IsValid);
    //    Assert.False(_departmentService.IsValid);
    //    Assert.Single(_departmentService.Notifications);
    //    Assert.Equal(Error.Department.ERRO_AO_REMOVER, _departmentService.Notifications.ElementAt(0).Message);

    //    _departmentRepositoryMock
    //        .Verify(x => x.RemoveDepartmentAsync(It.IsAny<Department>()), Times.Once);

    //    _departmentRepositoryMock
    //        .Verify(x => x.GetDepartmentByIdAsync(It.IsAny<Guid>()), Times.Once);
    //}

    //[Fact]
    //public async Task RemoveDepartmentAsync_ShouldReturnTrue()
    //{
    //    // Arrange
    //    var Department = new Department("Engenharia de Produtos");
    //    _departmentRepositoryMock.Setup(repo => repo.RemoveDepartmentAsync(Department))
    //        .ReturnsAsync(true);
    //    _departmentRepositoryMock.Setup(repo => repo.GetDepartmentByIdAsync(Department.Id))
    //        .ReturnsAsync(Department);

    //    // Act
    //    var result = await _departmentService.RemoveDepartmentAsync(Department);

    //    Assert.True(result);
    //    Assert.True(Department.IsValid);
    //    Assert.True(_departmentService.IsValid);

    //    _departmentRepositoryMock
    //        .Verify(x => x.RemoveDepartmentAsync(It.IsAny<Department>()), Times.Once);

    //    _departmentRepositoryMock
    //        .Verify(x => x.GetDepartmentByIdAsync(It.IsAny<Guid>()), Times.Once);
    //}
    
}

