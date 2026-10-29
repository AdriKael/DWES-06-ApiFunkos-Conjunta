namespace ApiFunkosConjunta.Tests.Integration;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using ApiFunkosConjunta.Dtos;

[TestFixture]
public class FunkosIntegrationTests
{
    private PostgreSqlContainer _postgres = null!;
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _postgres = new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("funkos")
            .WithUsername("admin")
            .WithPassword("admin123")
            .Build();
        await _postgres.StartAsync();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Development");
                builder.UseSetting("ConnectionStrings:DefaultConnection", _postgres.GetConnectionString());
            });

        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
        await _postgres.DisposeAsync();
    }

    [Test]
    public async Task CreateAndGet_Funko_Works()
    {
        // Create category first
        var catResponse = await _client.PostAsJsonAsync("/api/categorias", new CreateCategoriaDto { Nombre = "Marvel" });
        catResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var categoria = await catResponse.Content.ReadFromJsonAsync<CategoriaDto>();
        categoria.Should().NotBeNull();

        var createDto = new CreateFunkoDto { Nombre = "Spiderman", Precio = 15.5m, Stock = 10, CategoriaId = categoria!.Id };
        var createResponse = await _client.PostAsJsonAsync("/api/funkos", createDto);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResponse.Content.ReadFromJsonAsync<FunkoDto>();
        created.Should().NotBeNull();
        created!.Nombre.Should().Be("Spiderman");

        var getResponse = await _client.GetAsync($"/api/funkos/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var fetched = await getResponse.Content.ReadFromJsonAsync<FunkoDto>();
        fetched!.Nombre.Should().Be("Spiderman");
    }

    [Test]
    public async Task GetAll_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/funkos");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Delete_Funko_ReturnsNoContent()
    {
        var catResponse = await _client.PostAsJsonAsync("/api/categorias", new CreateCategoriaDto { Nombre = "DC Comics" });
        catResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var categoria = await catResponse.Content.ReadFromJsonAsync<CategoriaDto>();

        var createResponse = await _client.PostAsJsonAsync("/api/funkos", new CreateFunkoDto { Nombre = "Batman", Precio = 12, Stock = 3, CategoriaId = categoria!.Id });
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResponse.Content.ReadFromJsonAsync<FunkoDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/funkos/{created!.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task SearchByName_ReturnsMatches()
    {
        var response = await _client.GetAsync("/api/funkos/search?name=Spider");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
