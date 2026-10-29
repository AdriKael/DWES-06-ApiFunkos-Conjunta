namespace ApiFunkosConjunta.Tests.Unit.Services;

using Moq;
using FluentAssertions;
using ApiFunkosConjunta.Dtos;
using ApiFunkosConjunta.Errors;
using ApiFunkosConjunta.Errors.Funkos;
using ApiFunkosConjunta.Models;
using ApiFunkosConjunta.Repositories;
using ApiFunkosConjunta.Services;
using ApiFunkosConjunta.Services.Cache;
using ApiFunkosConjunta.Validators;

[TestFixture]
public class FunkoServiceTests
{
    private Mock<IFunkoRepository> _repo = null!;
    private Mock<IFunkoValidator> _validator = null!;
    private Mock<ICacheService> _cache = null!;
    private FunkoService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _repo = new Mock<IFunkoRepository>();
        _validator = new Mock<IFunkoValidator>();
        _cache = new Mock<ICacheService>();
        _sut = new FunkoService(_repo.Object, _validator.Object, _cache.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<FunkoService>>());
    }

    [Test]
    public async Task GetById_WhenExists_ReturnsFunko()
    {
        var funko = new Funko { Id = 1, Nombre = "Batman", Precio = 10, Stock = 5, CategoriaId = 1, Categoria = new Categoria { Id = 1, Nombre = "DC" } };
        _cache.Setup(c => c.GetAsync<FunkoDto>(It.IsAny<string>())).ReturnsAsync((FunkoDto?)null);
        _repo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(funko);

        var result = await _sut.GetByIdAsync(1);

        result.IsSuccess.Should().BeTrue();
        result.Value.Nombre.Should().Be("Batman");
    }

    [Test]
    public async Task GetById_WhenNotExists_ReturnsNotFound()
    {
        _cache.Setup(c => c.GetAsync<FunkoDto>(It.IsAny<string>())).ReturnsAsync((FunkoDto?)null);
        _repo.Setup(r => r.FindByIdAsync(99)).ReturnsAsync((Funko?)null);

        var result = await _sut.GetByIdAsync(99);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundError>();
    }

    [Test]
    public async Task Create_WithDuplicateName_ReturnsConflict()
    {
        var dto = new CreateFunkoDto { Nombre = "Batman", Precio = 10, Stock = 5, CategoriaId = 1 };
        _validator.Setup(v => v.ValidateCreateAsync(dto)).ReturnsAsync(FunkoError.NombreDuplicado(dto.Nombre));

        var result = await _sut.CreateAsync(dto);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ConflictError>();
    }

    [Test]
    public async Task Delete_WhenNotExists_ReturnsNotFound()
    {
        _repo.Setup(r => r.FindByIdAsync(99)).ReturnsAsync((Funko?)null);

        var result = await _sut.DeleteAsync(99);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundError>();
    }
}
