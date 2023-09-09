namespace PontoLegal.Test.PontoLegal.Service;

public class CargoServiceTest
{
    private readonly Mock<ICargoRepository> _cargoRepositoryMock;
    private readonly CargoService _cargoService;

    public CargoServiceTest()
    {
        _cargoRepositoryMock = new Mock<ICargoRepository>();
        _cargoService = new CargoService(_cargoRepositoryMock.Object);
    }

    [Theory]
    [InlineData("", "Desenvolvimento")]
    [InlineData("D", "Desenvolvimento")]
    [InlineData("Desenvolvedor com mais de trinta caracteres no nome", "Desenvolvimento")]
    public async Task AddCargoAsync_ShouldReturnFalseWithError_WhenInvalidCargoName(string nome, string departamento)
    {
        // Arrange
        var cargo = new Cargo(nome, new Departamento(departamento));
        _cargoRepositoryMock.Setup(x => x.AddCargoAsync(cargo))
            .ReturnsAsync(false);

        // Act
        var result = await _cargoService.AddCargoAsync(cargo);

        // Assert
        Assert.False(cargo.IsValid);
        Assert.False(result);

        _cargoRepositoryMock.Verify(x => x.AddCargoAsync(cargo), Times.Never);
    }
}
