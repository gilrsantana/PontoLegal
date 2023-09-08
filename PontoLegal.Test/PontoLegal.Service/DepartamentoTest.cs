using Moq;
using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service;
using PontoLegal.Service.Interfaces;

namespace PontoLegal.Test.PontoLegal.Service;

public class DepartamentoServiceTest
{
    private readonly Mock<IDepartamentoRepository> _departamentoRepositoryMock;
    private readonly IDepartamentoService _departamentoService;

    public DepartamentoServiceTest()
    {
        _departamentoRepositoryMock = new Mock<IDepartamentoRepository>();
        _departamentoService = new DepartamentoService(_departamentoRepositoryMock.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData("D")]
    [InlineData("Departamento de Engenharia de Produtos")]
    public async Task AddDepartamentoAsync_ShouldReturnNullWithErrors_WithInvalidName(string nome)
    {
        // Arrange
        var departamento = new Departamento(nome);
        _departamentoRepositoryMock.Setup(repo => repo.AddDepartamentoAsync(departamento))
            .ReturnsAsync(true);

        // Act
        var result = await _departamentoService.AddDepartamentoAsync(departamento);

        // Assert
        
        Assert.False(result);
        Assert.False(departamento.IsValid());
        Assert.False(_departamentoService.IsValid());
        Assert.Equal("Nome do Departamento deve ter entre 3 e 30 caracteres", departamento.Errors[0]);

        _departamentoRepositoryMock
            .Verify(x => x.AddDepartamentoAsync(It.IsAny<Departamento>()), Times.Never);
    }
}

