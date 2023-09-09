namespace PontoLegal.Test.PontoLegal.Service;

public class DepartamentoServiceTest
{
    private readonly Mock<IDepartamentoRepository> _departamentoRepositoryMock;
    private readonly DepartamentoService _departamentoService;

    public DepartamentoServiceTest()
    {
        _departamentoRepositoryMock = new Mock<IDepartamentoRepository>();
        _departamentoService = new DepartamentoService(_departamentoRepositoryMock.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData("D")]
    [InlineData("Departamento de Engenharia de Produtos")]
    public async Task AddDepartamentoAsync_ShouldReturnFalseWithErrors_WithInvalidName(string nome)
    {
        // Arrange
        var departamento = new Departamento(nome);
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
            .Verify(x => x.AddDepartamentoAsync(It.IsAny<Departamento>()), Times.Never);
    }

    [Fact]
    public async Task AddDepartamentoAsync_ShouldReturnTrue_WithValidName()
    {
        // Arrange
        var departamento = new Departamento("Engenharia de Produtos");
        _departamentoRepositoryMock.Setup(repo => repo.AddDepartamentoAsync(departamento))
            .ReturnsAsync(true);

        // Act
        var result = await _departamentoService.AddDepartamentoAsync(departamento);

        // Assert
        Assert.True(result);
        Assert.True(departamento.IsValid);
        Assert.True(_departamentoService.IsValid);
        Assert.Empty(_departamentoService.Notifications);

        _departamentoRepositoryMock
            .Verify(x => x.AddDepartamentoAsync(It.IsAny<Departamento>()), Times.Once);
    }
}

