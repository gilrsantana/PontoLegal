using PontoLegal.Service.Entities;

namespace PontoLegal.Test.PontoLegal.Service;

public class JobPositionServiceTest
{
    private readonly Mock<IJobPositionRepository> _jobPositionRepositoryMock;
    private readonly JobPositionService _jobPositionService;

    public JobPositionServiceTest()
    {
        _jobPositionRepositoryMock = new Mock<IJobPositionRepository>();
        _jobPositionService = new JobPositionService(_jobPositionRepositoryMock.Object);
    }

    [Theory]
    [InlineData("", "Development")]
    [InlineData("D", "Development")]
    [InlineData("Development with more than 30 chars in the name", "Development")]
    public async Task AddJobPositionAsync_ShouldReturnFalseWithError_WhenInvalidJobPositionName(string name, string department)
    {
        // Arrange
        var jobPosition = new JobPosition(name, new Department(department));
        var model = new JobPositionModel(name, new DepartmentModel(department));
        _jobPositionRepositoryMock.Setup(x => x.AddJobPositionAsync(jobPosition))
            .ReturnsAsync(false);

        // Act
        var result = await _jobPositionService.AddJobPositionAsync(model);

        // Assert
        Assert.False(model.IsValid);
        Assert.False(result);
    }
}
