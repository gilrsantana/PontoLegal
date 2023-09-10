namespace PontoLegal.Test.PontoLegal.Service;

public class DepartamentoServiceTest
{
    private readonly Mock<IDepartmentRepository> _departamentoRepositoryMock;
    private readonly DepartmentService _departamentoService;

    public DepartamentoServiceTest()
    {
        _departamentoRepositoryMock = new Mock<IDepartmentRepository>();
        _departamentoService = new DepartmentService(_departamentoRepositoryMock.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData("D")]
    [InlineData("Departamento de Engenharia de Produtos")]
    public async Task AddDepartamentoAsync_ShouldReturnFalseWithErrors_WithInvalidName(string nome)
    {
        // Arrange
        var departamento = new Department(nome);
        _departamentoRepositoryMock.Setup(repo => repo.AddDepartamentoAsync(departamento))
            .ReturnsAsync(true);

        // Act
        var result = await _departamentoService.AddDepartamentoAsync(departamento);

        // Assert
        
        Assert.False(result);
        Assert.False(departamento.IsValid);
        Assert.False(_departamentoService.IsValid);
        Assert.Equal(1, _departamentoService.Notifications.Count);
        Assert.Equal(Error.Departamento.NOME_INVALIDO, _departamentoService.Notifications.ElementAt(0).Message);

        _departamentoRepositoryMock
            .Verify(x => x.AddDepartamentoAsync(It.IsAny<Department>()), Times.Never);
    }

    [Fact]
    public async Task AddDepartamentoAsync_ShouldReturnFalseWithErrors_WithRepeatedName()
    {
               // Arrange
        var departamento = new Department("Engenharia de Produtos");
        _departamentoRepositoryMock.Setup(repo => repo.AddDepartamentoAsync(departamento))
            .ReturnsAsync(true);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByNomeAsync(departamento.Nome))
            .ReturnsAsync(departamento);

        // Act
        var result = await _departamentoService.AddDepartamentoAsync(departamento);

        // Assert
        Assert.False(result);
        Assert.True(departamento.IsValid);
        Assert.False(_departamentoService.IsValid);
        Assert.Single(_departamentoService.Notifications);
        Assert.Equal(Error.Departamento.NOME_JA_EXISTE, _departamentoService.Notifications.ElementAt(0).Message);

        _departamentoRepositoryMock
            .Verify(x => x.AddDepartamentoAsync(It.IsAny<Department>()), Times.Never);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByNomeAsync(It.IsAny<string>()), Times.Once);

    }

    [Fact]
    public async Task AddDepartamentoAsync_ShouldReturnFalseWithErrors_WithErrorInRepository()
    {
        // Arrange
        var departamento = new Department("Engenharia de Produtos");
        _departamentoRepositoryMock.Setup(repo => repo.AddDepartamentoAsync(departamento))
            .ReturnsAsync(false);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByNomeAsync(departamento.Nome))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _departamentoService.AddDepartamentoAsync(departamento);

        // Assert
        Assert.False(result);
        Assert.True(departamento.IsValid);
        Assert.False(_departamentoService.IsValid);
        Assert.Single(_departamentoService.Notifications);
        Assert.Equal(Error.Departamento.ERRO_AO_ADICIONAR, _departamentoService.Notifications.ElementAt(0).Message);

        _departamentoRepositoryMock
            .Verify(x => x.AddDepartamentoAsync(It.IsAny<Department>()), Times.Once);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByNomeAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task AddDepartamentoAsync_ShouldReturnTrue_WithValidName()
    {
        // Arrange
        var departamento = new Department("Engenharia de Produtos");
        _departamentoRepositoryMock.Setup(repo => repo.AddDepartamentoAsync(departamento))
            .ReturnsAsync(true);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByNomeAsync(departamento.Nome))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _departamentoService.AddDepartamentoAsync(departamento);

        // Assert
        Assert.True(result);
        Assert.True(departamento.IsValid);
        Assert.True(_departamentoService.IsValid);
        Assert.Empty(_departamentoService.Notifications);

        _departamentoRepositoryMock
            .Verify(x => x.AddDepartamentoAsync(It.IsAny<Department>()), Times.Once);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByNomeAsync(It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("D")]
    [InlineData("Departamento de Engenharia de Produtos")]
    public async Task UpdateDepartamentoAsync_ShouldReturnFalse_WithInvalidName(string nome)
    {
        // Arrange
        var departamento = new Department(nome);
        _departamentoRepositoryMock.Setup(repo => repo.UpdateDepartamentoAsync(departamento))
            .ReturnsAsync(true);

        // Act
        var result = await _departamentoService.UpdateDepartamentoAsync(departamento);

        // Assert
        Assert.False(result);
        Assert.False(departamento.IsValid);
        Assert.False(_departamentoService.IsValid);
        Assert.Single(_departamentoService.Notifications);
        Assert.Equal(Error.Departamento.NOME_INVALIDO, _departamentoService.Notifications.ElementAt(0).Message);

        _departamentoRepositoryMock
            .Verify(x => x.UpdateDepartamentoAsync(It.IsAny<Department>()), Times.Never);
    }

    [Fact]
    public async Task UpdateDepartamentoAsync_ShouldReturnFalseWithErrors_WithRepeatedName()
    {
        // Arrange
        var departamento1 = new Department("Engenharia de Produtos");
        var departamento2 = new Department("Engenharia de Produtos");
        _departamentoRepositoryMock.Setup(repo => repo.UpdateDepartamentoAsync(departamento1))
            .ReturnsAsync(true);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByNomeAsync(departamento2.Nome))
            .ReturnsAsync(departamento2);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByIdAsync(departamento1.Id))
            .ReturnsAsync(departamento1);

        // Act
        var result = await _departamentoService.UpdateDepartamentoAsync(departamento1);

        // Assert
        Assert.False(result);
        Assert.True(departamento1.IsValid);
        Assert.False(_departamentoService.IsValid);
        Assert.Single(_departamentoService.Notifications);
        Assert.Equal(Error.Departamento.NOME_JA_EXISTE, _departamentoService.Notifications.ElementAt(0).Message);

        _departamentoRepositoryMock
            .Verify(x => x.UpdateDepartamentoAsync(It.IsAny<Department>()), Times.Never);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByNomeAsync(It.IsAny<string>()), Times.Once);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDepartamentoAsync_ShouldReturnFalseWithErrors_WithIdNotExists()
    {
        // Arrange
        var departamento = new Department("Engenharia de Produtos");
        _departamentoRepositoryMock.Setup(repo => repo.UpdateDepartamentoAsync(departamento))
            .ReturnsAsync(true);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByNomeAsync(departamento.Nome))
            .ReturnsAsync((Department?)null);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByIdAsync(departamento.Id))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _departamentoService.UpdateDepartamentoAsync(departamento);

        // Assert
        Assert.False(result);
        Assert.True(departamento.IsValid);
        Assert.False(_departamentoService.IsValid);
        Assert.Single(_departamentoService.Notifications);
        Assert.Equal(Error.Departamento.DEPARTAMENTO_NAO_ENCONTRADO, _departamentoService.Notifications.ElementAt(0).Message);

        _departamentoRepositoryMock
            .Verify(x => x.UpdateDepartamentoAsync(It.IsAny<Department>()), Times.Never);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByNomeAsync(It.IsAny<string>()), Times.Once);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByIdAsync(It.IsAny<Guid>()), Times.Once);
    
    }

    [Fact]
    public async Task UpdateDepartamentoAsync_ShouldReturnFalseWithErrors_WithErrorInRepository()
    {
        // Arrange
        var departamento = new Department("Engenharia de Produtos");
        _departamentoRepositoryMock.Setup(repo => repo.UpdateDepartamentoAsync(departamento))
            .ReturnsAsync(false);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByNomeAsync(departamento.Nome))
            .ReturnsAsync((Department?)null);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByIdAsync(departamento.Id))
            .ReturnsAsync(departamento);

        // Act
        var result = await _departamentoService.UpdateDepartamentoAsync(departamento);

        // Assert
        Assert.False(result);
        Assert.True(departamento.IsValid);
        Assert.False(_departamentoService.IsValid);
        Assert.Single(_departamentoService.Notifications);
        Assert.Equal(Error.Departamento.ERRO_AO_ATUALIZAR, _departamentoService.Notifications.ElementAt(0).Message);

        _departamentoRepositoryMock
            .Verify(x => x.UpdateDepartamentoAsync(It.IsAny<Department>()), Times.Once);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByNomeAsync(It.IsAny<string>()), Times.Once);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByIdAsync(It.IsAny<Guid>()), Times.Once);
    
    }

    [Fact]
    public async Task UpdateDepartamentoAsync_ShouldReturnTrue_WithValidName()
    {
        // Arrange
        var departamento = new Department("Engenharia de Produtos");
        _departamentoRepositoryMock.Setup(repo => repo.UpdateDepartamentoAsync(departamento))
            .ReturnsAsync(true);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByNomeAsync(departamento.Nome))
            .ReturnsAsync((Department?)null);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByIdAsync(departamento.Id))
            .ReturnsAsync(departamento);

        // Act
        var result = await _departamentoService.UpdateDepartamentoAsync(departamento);

        // Assert
        Assert.True(result);
        Assert.True(departamento.IsValid);
        Assert.True(_departamentoService.IsValid);
        Assert.Empty(_departamentoService.Notifications);

        _departamentoRepositoryMock
            .Verify(x => x.UpdateDepartamentoAsync(It.IsAny<Department>()), Times.Once);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByNomeAsync(It.IsAny<string>()), Times.Once);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task RemoveDepartamentoAsync_ShouldReturnFalseWithErrors_WithIdNotExists()
    {
        // Arrange
        var departamento = new Department("Engenharia de Produtos");
        _departamentoRepositoryMock.Setup(repo => repo.RemoveDepartamentoAsync(departamento))
            .ReturnsAsync(true);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByIdAsync(departamento.Id))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _departamentoService.RemoveDepartamentoAsync(departamento);

        // Assert
        Assert.False(result);
        Assert.True(departamento.IsValid);
        Assert.False(_departamentoService.IsValid);
        Assert.Single(_departamentoService.Notifications);
        Assert.Equal(Error.Departamento.DEPARTAMENTO_NAO_ENCONTRADO, _departamentoService.Notifications.ElementAt(0).Message);

        _departamentoRepositoryMock
            .Verify(x => x.RemoveDepartamentoAsync(It.IsAny<Department>()), Times.Never);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task RemoveDepartamentoAsync_ShouldReturnReturnFalseWithErrors_WithErrorInRepository()
    {
        // Arrange
        var departamento = new Department("Engenharia de Produtos");
        _departamentoRepositoryMock.Setup(repo => repo.RemoveDepartamentoAsync(departamento))
            .ReturnsAsync(false);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByIdAsync(departamento.Id))
            .ReturnsAsync(departamento);

        // Act
        var result = await _departamentoService.RemoveDepartamentoAsync(departamento);

        // Assert
        Assert.False(result);
        Assert.True(departamento.IsValid);
        Assert.False(_departamentoService.IsValid);
        Assert.Single(_departamentoService.Notifications);
        Assert.Equal(Error.Departamento.ERRO_AO_REMOVER, _departamentoService.Notifications.ElementAt(0).Message);

        _departamentoRepositoryMock
            .Verify(x => x.RemoveDepartamentoAsync(It.IsAny<Department>()), Times.Once);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task RemoveDepartamentoAsync_ShouldReturnTrue()
    {
        // Arrange
        var departamento = new Department("Engenharia de Produtos");
        _departamentoRepositoryMock.Setup(repo => repo.RemoveDepartamentoAsync(departamento))
            .ReturnsAsync(true);
        _departamentoRepositoryMock.Setup(repo => repo.GetDepartamentoByIdAsync(departamento.Id))
            .ReturnsAsync(departamento);

        // Act
        var result = await _departamentoService.RemoveDepartamentoAsync(departamento);

        Assert.True(result);
        Assert.True(departamento.IsValid);
        Assert.True(_departamentoService.IsValid);

        _departamentoRepositoryMock
            .Verify(x => x.RemoveDepartamentoAsync(It.IsAny<Department>()), Times.Once);

        _departamentoRepositoryMock
            .Verify(x => x.GetDepartamentoByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
    
}

