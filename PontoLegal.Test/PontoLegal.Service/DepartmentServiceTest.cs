using PontoLegal.Service.DTOs;
using Xunit.Sdk;

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
        var model = new DepartmentModel(name);
        _departmentRepositoryMock
            .Setup(repo => repo.AddDepartmentAsync(It.IsAny<Department>()))
            .ReturnsAsync(true);

        // Act
        var result = await _departmentService.AddDepartmentAsync(model);

        // Assert
        Assert.False(result);
        Assert.False(model.IsValid);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.INVALID_NAME, _departmentService.Notifications.ElementAt(0).Message);
    }

    [Fact]
    public async Task AddDepartmentAsync_ShouldReturnFalseWithErrors_WithRepeatedName()
    {
        // Arrange
        var department = new Department("Products Development");
        var model = new DepartmentModel("Products Development");
        _departmentRepositoryMock
            .Setup(repo => repo.AddDepartmentAsync(It.IsAny<Department>()))
            .ReturnsAsync(true);
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByNameAsync(model.Name))
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
        var model = new DepartmentModel("Products Development");
        _departmentRepositoryMock
            .Setup(repo => repo.AddDepartmentAsync(It.IsAny<Department>()))
            .ReturnsAsync(false);

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
    public async Task AddDepartmentAsync_ShouldReturnTrue()
    {
        // Arrange
        var model = new DepartmentModel("Products Development");
        _departmentRepositoryMock
            .Setup(repo => repo.AddDepartmentAsync(It.IsAny<Department>()))
            .ReturnsAsync(true);

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
        _departmentRepositoryMock
            .Setup(repo => repo.UpdateDepartmentAsync(id, department))
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
        var model = new DepartmentModel("Products Development");
        var department1 = new Department(model.Name);
        var department2 = new Department(model.Name);
        var id = Guid.NewGuid();
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByNameAsync(model.Name))
            .ReturnsAsync(department2);
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByIdAsync(id))
            .ReturnsAsync(department1);
        _departmentRepositoryMock
            .Setup(repo => repo.UpdateDepartmentAsync(id, department1))
            .ReturnsAsync(true);

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
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByNameAsync(model.Name))
            .ReturnsAsync((Department?)null);
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByIdAsync(id))
            .ReturnsAsync(department);
        _departmentRepositoryMock
            .Setup(repo => repo.UpdateDepartmentAsync(id, department))
            .ReturnsAsync(false);

        // Act
        var result = await _departmentService.UpdateDepartmentAsync(id, model);

        // Assert
        Assert.False(result);
        Assert.True(model.IsValid);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.ERROR_UPDATING, _departmentService.Notifications.ElementAt(0).Message);
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ShouldReturnTrue()
    {
        // Arrange
        var model = new DepartmentModel("Products Development");
        var department = new Department(model.Name);
        var id = Guid.NewGuid();
        department.Id = id;
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByNameAsync(model.Name))
            .ReturnsAsync((Department?)null);
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByIdAsync(id))
            .ReturnsAsync(department);
        _departmentRepositoryMock
            .Setup(repo => repo.UpdateDepartmentAsync(It.IsAny<Guid>(), It.IsAny<Department>()))
            .ReturnsAsync(true);

        // Act
        var result = await _departmentService.UpdateDepartmentAsync(id, model);

        // Assert
        Assert.True(result);
        Assert.True(model.IsValid);
        Assert.True(_departmentService.IsValid);
        Assert.Empty(_departmentService.Notifications);
    }

    [Fact]
    public async Task RemoveDepartmentAsync_ShouldReturnFalseWithErrors_WithIdNotExists()
    {
        // Arrange
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Department?)null);
        _departmentRepositoryMock
            .Setup(repo => repo.RemoveDepartmentByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        // Act
        var result = await _departmentService.RemoveDepartmentByIdAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.DEPARTMENT_NOT_FOUNDED, _departmentService.Notifications.ElementAt(0).Message);
    }

    [Fact]
    public async Task RemoveDepartmentAsync_ShouldReturnReturnFalseWithErrors_WithEmptyGuid()
    {
        // Arrange
        var department = new Department("Products Development");
        var id = Guid.Empty;
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByIdAsync(id))
            .ReturnsAsync((Department?)null);
        _departmentRepositoryMock
            .Setup(repo => repo.RemoveDepartmentByIdAsync(id))
            .ReturnsAsync(false);

        // Act
        var result = await _departmentService.RemoveDepartmentByIdAsync(id);

        // Assert
        Assert.False(result);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.INVALID_ID, _departmentService.Notifications.ElementAt(0).Message);
    }
    
    [Fact]
    public async Task RemoveDepartmentAsync_ShouldReturnReturnFalseWithErrors_WithErrorInRepository()
    {
        // Arrange
        var department = new Department("Products Development");
        var id = department.Id;
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByIdAsync(id))
            .ReturnsAsync(department);
        _departmentRepositoryMock
            .Setup(repo => repo.RemoveDepartmentByIdAsync(id))
            .ReturnsAsync(false);

        // Act
        var result = await _departmentService.RemoveDepartmentByIdAsync(id);

        // Assert
        Assert.False(result);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.ERROR_REMOVING, _departmentService.Notifications.ElementAt(0).Message);
    }

    [Fact]
    public async Task RemoveDepartmentAsync_ShouldReturnTrue()
    {
        // Arrange
        var department = new Department("Products Development");
        var id = department.Id;
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByIdAsync(id))
            .ReturnsAsync(department);
        _departmentRepositoryMock
            .Setup(repo => repo.RemoveDepartmentByIdAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await _departmentService.RemoveDepartmentByIdAsync(id);

        Assert.True(result);
        Assert.True(_departmentService.IsValid);
        Assert.Empty(_departmentService.Notifications);
    }

    [Fact]
    public async Task GetDepartmentByNameAsync_ShouldReturnNull_WithEmptyName()
    {
        // Arrange
        var name = string.Empty;
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByNameAsync(name))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _departmentService.GetDepartmentByNameAsync(name);

        // Assert
        Assert.Null(result);
        Assert.True(_departmentService.IsValid);
        Assert.Empty(_departmentService.Notifications);
    }

    [Fact]
    public async Task GetDepartmentByNameAsync_ShouldReturnNull_WithNameNotFound()
    {
        // Arrange
        var name = "Department Name";
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByNameAsync(name))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _departmentService.GetDepartmentByNameAsync(name);

        // Assert
        Assert.Null(result);
        Assert.True(_departmentService.IsValid);
        Assert.Empty(_departmentService.Notifications);
    }

    [Fact]
    public async Task GetDepartmentByNameAsync_ShouldReturnDTO()
    {
        // Arrange
        var name = "Products Development";
        var model = new Department(name);
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(model);

        // Act
        var result = await _departmentService.GetDepartmentByNameAsync(name);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.Id, result.Id);
        Assert.Equal(model.Name, result.Name);
        Assert.IsType<DepartmentDTO?>(result);
        Assert.True(_departmentService.IsValid);
        Assert.Empty(_departmentService.Notifications);
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldReturnNull_WithEmptyGuid()
    {
        // Arrange
        var guid = Guid.Empty;
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByIdAsync(guid))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _departmentService.GetDepartmentByIdAsync(guid);

        // Assert
        Assert.Null(result);
        Assert.True(_departmentService.IsValid);
        Assert.Empty(_departmentService.Notifications);
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldReturnNull_WithGuidNotFound()
    {
        // Arrange
        var guid = Guid.NewGuid();
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByIdAsync(guid))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _departmentService.GetDepartmentByIdAsync(guid);

        // Assert
        Assert.Null(result);
        Assert.True(_departmentService.IsValid);
        Assert.Empty(_departmentService.Notifications);
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldReturnDTO()
    {
        // Arrange
        var name = "Products Development";
        var model = new Department(name);
        _departmentRepositoryMock
            .Setup(repo => repo.GetDepartmentByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(model);

        // Act
        var result = await _departmentService.GetDepartmentByIdAsync(Guid.NewGuid());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.Id, result.Id);
        Assert.Equal(model.Name, result.Name);
        Assert.IsType<DepartmentDTO?>(result);
        Assert.True(_departmentService.IsValid);
        Assert.Empty(_departmentService.Notifications);
    }

    [Theory]
    [InlineData(-1, 25)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(-1, 0)]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    public async Task GetAllDepartmentsAsync_ShouldReturnEmptyList_WithInvalidSkipTake(int skip, int take)
    {
        // Arrange
        _departmentRepositoryMock
            .Setup(repo => repo.GetAllDepartmentsAsync(skip, take))
            .ReturnsAsync((ICollection<Department>?)null);

        // Act
        var result = await _departmentService.GetAllDepartmentsAsync(skip, take);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.INVALID_PAGINATION, _departmentService.Notifications.First().Message);
    }

    [Fact]
    public async Task GetAllDepartmentsAsync_ShouldReturnEmptyList_WithNoDepartments()
    {
        // Arrange
        _departmentRepositoryMock
            .Setup(repo => repo.GetAllDepartmentsAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((ICollection<Department>?)null);

        // Act
        var result = await _departmentService.GetAllDepartmentsAsync(0, 25);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.True(_departmentService.IsValid);
        Assert.Empty(_departmentService.Notifications);
    }

    [Theory]
    [InlineData(0, 25)]
    [InlineData(0, 1)]
    [InlineData(0, 50)]
    [InlineData(25, 1)]
    [InlineData(25, 25)]
    [InlineData(1, 30)]
    public async Task GetAllDepartmentsAsync_ShouldReturnEmptyList_WithRepositoryError(int skip, int take)
    {
        // Arrange
        _departmentRepositoryMock
            .Setup(repo => repo.GetAllDepartmentsAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((ICollection<Department>?)null);

        // Act
        var result = await _departmentService.GetAllDepartmentsAsync(skip, take);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.False(_departmentService.IsValid);
        Assert.Single(_departmentService.Notifications);
        Assert.Equal(Error.Department.ERROR_GETTING_ALL, _departmentService.Notifications.First().Message);
    }

    [Fact]
    public async Task GetAllDepartmentsAsync_ShouldReturnNotEmptyLis()
    {
                // Arrange
        var model = new List<Department>
        {
            new Department("Department 1"),
            new Department("Department 2"),
            new Department("Department 3"),
            new Department("Department 4"),
            new Department("Department 5")
        };
        _departmentRepositoryMock
            .Setup(repo => repo.GetAllDepartmentsAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(model);

        // Act
        var result = await _departmentService.GetAllDepartmentsAsync(0, 25);

        // Assert
        Assert.True(_departmentService.IsValid);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(model.Count, result.Count);
    }
}

