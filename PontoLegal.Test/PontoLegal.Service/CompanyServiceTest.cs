namespace PontoLegal.Test.PontoLegal.Service;

public class CompanyServiceTest
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly CompanyService _companyService;

    public CompanyServiceTest()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _companyService = new CompanyService(_companyRepositoryMock.Object);
    }
    
    #region GetCompanyById
    [Fact]
    public async Task GetCompanyById_ShouldReturnsNullWithError_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;
        
        // Act
        var result = await _companyService.GetCompanyByIdAsync(id);
        
        // Assert
        Assert.Null(result);
        Assert.False(_companyService.IsValid);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.ID_IS_REQUIRED, _companyService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task GetCompanyById_ShouldReturnsNull_WithUnknownId()
    {
        // Arrange
        var id = Guid.NewGuid();
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(id))
            .ReturnsAsync((Company?)null);
        
        // Act
        var result = await _companyService.GetCompanyByIdAsync(id);
        
        // Assert
        Assert.Null(result);
        Assert.True(_companyService.IsValid);
        Assert.Empty(_companyService.Notifications);
    }
    
    [Fact]
    public async Task GetCompanyById_ShouldReturnsDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var company = new Company("Company", new Cnpj(MockCnpj.ValidCnpj));
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(id))
            .ReturnsAsync(company);
        
        // Act
        var result = await _companyService.GetCompanyByIdAsync(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<CompanyDTO?>(result);
        Assert.Equal(company.Id, result.Id);
        Assert.Equal(company.Name, result.Name);
        Assert.Equal(company.Cnpj, result.Cnpj);
        Assert.True(_companyService.IsValid);
        Assert.Empty(_companyService.Notifications);
    }
    
    #endregion
    
    #region GetCompanyByCnpjAsync
    
    [Fact]
    public async Task GetCompanyByCnpjAsync_ShouldReturnsNullWithError_WithInvalidCnpj()
    {
        // Arrange
        var cnpj = "123";
        
        // Act
        var result = await _companyService.GetCompanyByCnpjAsync(cnpj);
        
        // Assert
        Assert.Null(result);
        Assert.False(_companyService.IsValid);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Cnpj.INVALID_CNPJ_FORMAT, _companyService.Notifications.First().Message);
        Assert.Equal("CompanyService.Cnpj", _companyService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task GetCompanyByCnpjAsync_ShouldReturnsNullWithError_WithUnknownCnpj()
    {
        // Arrange
        var cnpj = new Cnpj(MockCnpj.ValidCnpj);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByCnpjAsync(cnpj))
            .ReturnsAsync((Company?)null);
        
        // Act
        var result = await _companyService.GetCompanyByCnpjAsync(MockCnpj.ValidCnpj);
        
        // Assert
        Assert.Null(result);
        Assert.True(_companyService.IsValid);
        Assert.Empty(_companyService.Notifications);
    }
    
    [Fact]
    public async Task GetCompanyByCnpjAsync_ShouldReturnsDto()
    {
        // Arrange
        var cnpj = new Cnpj(MockCnpj.ValidCnpj);
        var company = new Company("Company", cnpj);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByCnpjAsync(It.IsAny<Cnpj>()))
            .ReturnsAsync(company);
        
        // Act
        var result = await _companyService.GetCompanyByCnpjAsync(MockCnpj.ValidCnpj);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<CompanyDTO?>(result);
        Assert.Equal(company.Id, result.Id);
        Assert.Equal(company.Name, result.Name);
        Assert.Equal(company.Cnpj, result.Cnpj);
        Assert.True(_companyService.IsValid);
        Assert.Empty(_companyService.Notifications);
    }
    #endregion
    
    #region GetCompanyByNameAsync
    [Fact]
    public async Task GetCompanyByNameAsync_ShouldReturnsNullWithError_WithInvalidName()
    {
        // Arrange
        var name = "";
        
        // Act
        var result = await _companyService.GetCompanyByNameAsync(name);
        
        // Assert
        Assert.Null(result);
        Assert.False(_companyService.IsValid);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.NAME_IS_REQUIRED, _companyService.Notifications.First().Message);
    }

    [Fact]
    public async Task GetCompanyByNameAsync_ShouldReturnsDto()
    {
        // Arrange
        var name = "My Company";
        var company = new Company(name, new Cnpj(MockCnpj.ValidCnpj));
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(name))
            .ReturnsAsync(company);
        
        // Act
        var result = await _companyService.GetCompanyByNameAsync(name);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<CompanyDTO?>(result);
        Assert.Equal(company.Id, result.Id);
        Assert.Equal(company.Name, result.Name);
        Assert.Equal(company.Cnpj, result.Cnpj);
        Assert.True(_companyService.IsValid);
        Assert.Empty(_companyService.Notifications);
    }
    
    #endregion
    
    #region GetAllCompaniesAsync
    
    [Theory]
    [InlineData(-1, 25)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(-1, 0)]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    public async Task GetAllCompaniesAsync_ShouldReturnsEmptyList_WithInvalidSkipTake(int skip, int take)
    {
        // Arrange
        _companyRepositoryMock
            .Setup(repo => repo.GetAllCompaniesAsync(skip, take))
            .ReturnsAsync((ICollection<Company>?)null);
        
        // Act
        var result = await _companyService.GetAllCompaniesAsync(skip, take);
        
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.False(_companyService.IsValid);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.INVALID_PAGINATION, _companyService.Notifications.First().Message);
        Assert.Equal("CompanyService", _companyService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task GetAllCompaniesAsync_ShouldReturnsEmptyList_WithUnknownCompanies()
    {
        // Arrange
        var companies = new List<Company>();
        _companyRepositoryMock
            .Setup(repo => repo.GetAllCompaniesAsync(0, 25))
            .ReturnsAsync(companies);
        
        // Act
        var result = await _companyService.GetAllCompaniesAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.True(_companyService.IsValid);
        Assert.Empty(_companyService.Notifications);
    }
    
    [Fact]
    public async Task GetAllCompaniesAsync_ShouldReturnsEmptyList_WithRepositoryError()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(repo => repo.GetAllCompaniesAsync(0, 25))
            .ReturnsAsync((ICollection<Company>?)null);
        
        // Act
        var result = await _companyService.GetAllCompaniesAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.True(_companyService.IsValid);
        Assert.Empty(_companyService.Notifications);
    } 
    
    [Fact]
    public async Task GetAllCompaniesAsync_ShouldReturnsDtoList()
    {
        // Arrange
        var companies = new List<Company>
        {
            new("Company 1", new Cnpj(MockCnpj.ValidCnpj)),
            new("Company 2", new Cnpj(MockCnpj.ValidCnpj)),
            new("Company 3", new Cnpj(MockCnpj.ValidCnpj)),
        };
        _companyRepositoryMock
            .Setup(repo => repo.GetAllCompaniesAsync(0, 25))
            .ReturnsAsync(companies);
        
        // Act
        var result = await _companyService.GetAllCompaniesAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.IsType<List<CompanyDTO>>(result);
        Assert.Equal(companies.Count, result.Count);
        Assert.True(_companyService.IsValid);
        Assert.Empty(_companyService.Notifications);
    }
    
    #endregion
    
    #region AddCompanyAsync
    [Theory]
    [InlineData("")]
    [InlineData("C")]
    [InlineData("Name of Company with more than 30 characters")]
    public async Task AddCompanyAsync_ShouldReturnsFalseWithError_WithInvalidName(string name)
    {
        // Arrange
        var model = new CompanyModel(name, MockCnpj.ValidCnpj);
        
        // Act
        var result = await _companyService.AddCompanyAsync(model);
        
        // Assert
        Assert.False(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Single(model.Notifications);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.INVALID_NAME, model.Notifications.First().Message);
        Assert.Equal(Error.Company.INVALID_NAME, _companyService.Notifications.First().Message);
    }

    [Fact]
    public async Task AddCompanyAsync_ShouldReturnsFalseWithError_WithEmptyCnpj()
    {
        // Arrange
        var model = new CompanyModel("Company", "");
        
        // Act
        var result = await _companyService.AddCompanyAsync(model);
        
        // Assert
        Assert.False(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Single( model.Notifications);
        Assert.Single( _companyService.Notifications);
        Assert.Equal(Error.Company.INVALID_CNPJ, model.Notifications.First().Message);
        Assert.Equal(Error.Company.INVALID_CNPJ, _companyService.Notifications.First().Message);
    }
    
    [Theory]
    [InlineData("Company", "123")]
    [InlineData("Company", "1239012345678901234567890")]
    [InlineData("Company", "94.664.304/0001-05")]
    [InlineData("Company", "9466430400010A")]
    public async Task AddCompanyAsync_ShouldReturnsFalseWithError_WithInvalidCnpj(string name, string cnpj)
    {
        // Arrange
        var model = new CompanyModel(name, cnpj);
        
        // Act
        var result = await _companyService.AddCompanyAsync(model);
        
        // Assert
        Assert.True(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Single( _companyService.Notifications);
        Assert.Equal(Error.Cnpj.INVALID_CNPJ_FORMAT, _companyService.Notifications.First().Message);
        Assert.Equal("CompanyService.Cnpj", _companyService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task AddCompanyAsync_ShouldReturnsFalseWithError_ExistentCompanyByName()
    {
        // Arrange
        var model = new CompanyModel("Company", MockCnpj.ValidCnpj);
        var cnpj = new Cnpj(MockCnpj.ValidCnpj);
        var company = new Company(model.Name, cnpj);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByCnpjAsync(cnpj))
            .ReturnsAsync(company);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(model.Name))
            .ReturnsAsync(company);
        
        // Act
        var result = await _companyService.AddCompanyAsync(model);
        
        // Assert
        Assert.True(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.ALREADY_EXISTS, _companyService.Notifications.First().Message);
        Assert.Equal("CompanyService.Name", _companyService.Notifications.First().Key);
    }

    [Fact]
    public async Task AddCompanyAsync_ShouldReturnsFalseWithError_ExistentCompanyByCnpj()
    {
        // Arrange
        var model = new CompanyModel("Company", MockCnpj.ValidCnpj);
        var cnpj = new Cnpj(MockCnpj.ValidCnpj);
        var company = new Company(model.Name, cnpj);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByCnpjAsync(It.IsAny<Cnpj>()))
            .ReturnsAsync(company);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(model.Name))
            .ReturnsAsync((Company?)null);
        
        // Act
        var result = await _companyService.AddCompanyAsync(model);
        
        // Assert
        Assert.True(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.ALREADY_EXISTS, _companyService.Notifications.First().Message);
        Assert.Equal("CompanyService.Cnpj", _companyService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task AddCompanyAsync_ShouldReturnsFalseWithError_WithRepositoryError()
    {
        // Arrange
        var model = new CompanyModel("Company", MockCnpj.ValidCnpj);
        var cnpj = new Cnpj(MockCnpj.ValidCnpj);
        var company = new Company(model.Name, cnpj);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByCnpjAsync(cnpj))
            .ReturnsAsync((Company?)null);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(model.Name))
            .ReturnsAsync((Company?)null);
        _companyRepositoryMock
            .Setup(x => x.AddCompanyAsync(company))
            .ReturnsAsync(false);
        
        // Act
        var result = await _companyService.AddCompanyAsync(model);
        
        // Assert
        Assert.True(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.ERROR_ADDING, _companyService.Notifications.First().Message);
        Assert.Equal("CompanyService", _companyService.Notifications.First().Key);
    }
    
    [Fact]
    public async Task AddCompanyAsync_ShouldReturnsTrue()
    {
        // Arrange
        var model = new CompanyModel("Company", MockCnpj.ValidCnpj);
        var cnpj = new Cnpj(MockCnpj.ValidCnpj);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByCnpjAsync(cnpj))
            .ReturnsAsync((Company?)null);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(model.Name))
            .ReturnsAsync((Company?)null);
        _companyRepositoryMock
            .Setup(x => x.AddCompanyAsync(It.IsAny<Company>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _companyService.AddCompanyAsync(model);
        
        // Assert
        Assert.True(model.IsValid);
        Assert.True(_companyService.IsValid);
        Assert.True(result);
        Assert.Empty(_companyService.Notifications);
    }
    
    #endregion
    
    #region UpdateCompanyAsync
    
    [Fact]
    public async Task UpdateCompanyAsync_ShouldReturnsFalseWithError_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;
        var model = new CompanyModel("Company", MockCnpj.ValidCnpj);
        
        // Act
        var result = await _companyService.UpdateCompanyAsync(id, model);
        
        // Assert
        Assert.True(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.ID_IS_REQUIRED, _companyService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_ShouldReturnsFalseWithError_WithInvalidModel()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new CompanyModel("", "");
        
        // Act
        var result = await _companyService.UpdateCompanyAsync(id, model);
        
        // Assert
        Assert.False(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Equal(2, _companyService.Notifications.Count);
        Assert.Equal(Error.Company.INVALID_NAME, _companyService.Notifications.First().Message);
        Assert.Equal(Error.Company.INVALID_CNPJ, _companyService.Notifications.Last().Message);
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_ShouldReturnsFalseWithError_WithUnknownId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new CompanyModel("Company", MockCnpj.ValidCnpj);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(id))
            .ReturnsAsync((Company?)null);
        
        // Act
        var result = await _companyService.UpdateCompanyAsync(id, model);
        
        // Assert
        Assert.True(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.NOT_FOUNDED, _companyService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_ShouldReturnsFalseWithError_WithExistentCnpj()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new CompanyModel("Company", MockCnpj.ValidCnpj);
        var cnpj = new Cnpj(model.Cnpj);
        var company = new Company(model.Name, cnpj);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(id))
            .ReturnsAsync(company);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByCnpjAsync(It.IsAny<Cnpj>()))
            .ReturnsAsync(company);
        
        // Act
        var result = await _companyService.UpdateCompanyAsync(id, model);
        
        // Assert
        Assert.True(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.ALREADY_EXISTS, _companyService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_ShouldReturnsFalseWithError_WithExistentName()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new CompanyModel("Company", MockCnpj.ValidCnpj);
        var cnpj = new Cnpj(model.Cnpj);
        var company = new Company(model.Name, cnpj);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(id))
            .ReturnsAsync(company);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByCnpjAsync(cnpj))
            .ReturnsAsync((Company?)null);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(model.Name))
            .ReturnsAsync(company);
        
        // Act
        var result = await _companyService.UpdateCompanyAsync(id, model);
        
        // Assert
        Assert.True(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.ALREADY_EXISTS, _companyService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_ShouldReturnsFalseWithError_WithRepositoryError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new CompanyModel("Company", MockCnpj.ValidCnpj);
        var cnpj = new Cnpj(model.Cnpj);
        var company = new Company(model.Name, cnpj);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(id))
            .ReturnsAsync(company);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByCnpjAsync(cnpj))
            .ReturnsAsync((Company?)null);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(model.Name))
            .ReturnsAsync((Company?)null);
        _companyRepositoryMock
            .Setup(x => x.UpdateCompanyAsync(id, company))
            .ReturnsAsync(false);
        
        // Act
        var result = await _companyService.UpdateCompanyAsync(id, model);
        
        // Assert
        Assert.True(model.IsValid);
        Assert.False(_companyService.IsValid);
        Assert.False(result);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.ERROR_UPDATING, _companyService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_ShouldReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new CompanyModel("Company", MockCnpj.ValidCnpj);
        var cnpj = new Cnpj(model.Cnpj);
        var company = new Company(model.Name, cnpj);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(id))
            .ReturnsAsync(company);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByCnpjAsync(cnpj))
            .ReturnsAsync((Company?)null);
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(model.Name))
            .ReturnsAsync((Company?)null);
        _companyRepositoryMock
            .Setup(x => x.UpdateCompanyAsync(It.IsAny<Guid>(), It.IsAny<Company>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _companyService.UpdateCompanyAsync(id, model);
        
        // Assert
        Assert.True(model.IsValid);
        Assert.True(_companyService.IsValid);
        Assert.True(result);
        Assert.Empty(_companyService.Notifications);
    }
    #endregion
    
    #region RemoveCompanyAsync
    
    [Fact]
    public async Task RemoveCompanyAsync_ShouldReturnsFalseWithError_WithInvalidId()
    {
        // Arrange
        var id = Guid.Empty;
        
        // Act
        var result = await _companyService.RemoveCompanyByIdAsync(id);
        
        // Assert
        Assert.False(result);
        Assert.False(_companyService.IsValid);
        Assert.Single(_companyService.Notifications);
        Assert.Equal(Error.Company.ID_IS_REQUIRED, _companyService.Notifications.First().Message);
    }
    
    [Fact]
    public async Task RemoveCompanyAsync_ShouldReturnsFalse_WithUnknownId()
    {
        // Arrange
        var id = Guid.NewGuid();
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(id))
            .ReturnsAsync((Company?)null);
        
        // Act
        var result = await _companyService.RemoveCompanyByIdAsync(id);
        
        // Assert
        Assert.False(result);
        Assert.True(_companyService.IsValid);
        Assert.Empty(_companyService.Notifications);
    }
    
    [Fact]
    public async Task RemoveCompanyAsync_ShouldReturnsFalse_WithRepositoryError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var company = new Company("Company", new Cnpj(MockCnpj.ValidCnpj));
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(id))
            .ReturnsAsync(company);
        _companyRepositoryMock
            .Setup(x => x.RemoveCompanyByIdAsync(id))
            .ReturnsAsync(false);
        
        // Act
        var result = await _companyService.RemoveCompanyByIdAsync(id);
        
        // Assert
        Assert.False(result);
        Assert.True(_companyService.IsValid);
        Assert.Empty(_companyService.Notifications);
    }
    
    [Fact]
    public async Task RemoveCompanyAsync_ShouldReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var company = new Company("Company", new Cnpj(MockCnpj.ValidCnpj));
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(id))
            .ReturnsAsync(company);
        _companyRepositoryMock
            .Setup(x => x.RemoveCompanyByIdAsync(id))
            .ReturnsAsync(true);
        
        // Act
        var result = await _companyService.RemoveCompanyByIdAsync(id);
        
        // Assert
        Assert.True(result);
        Assert.True(_companyService.IsValid);
        Assert.Empty(_companyService.Notifications);
    }

    #endregion
}






