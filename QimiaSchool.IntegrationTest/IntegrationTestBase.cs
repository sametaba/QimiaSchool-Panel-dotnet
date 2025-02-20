using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using QimiaSchool.Common;
using QimiaSchool.DataAccess;
using System.Net.Http.Headers;
namespace QimiaSchool.IntegrationTests;
public abstract class IntegrationTestBase : IDisposable
{
    protected QimiaSchoolDbContext databaseContext;
    protected HttpClient client;
    private WebApplicationFactory<Program> _testWebAppFactory;
    protected IntegrationTestBase()
    {
    }
    [SetUp]
    public void Setup()
    {
        databaseContext.Database.EnsureCreated();
    }
    [TearDown]
    public void TearDown()
    {
        //databaseContext.Database.EnsureDeleted();
    }
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var auth0Configuration = GetAuth0Configuration();
        var connectionString = "Server=EUR;Database=QimiaSchoolDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;";
        _testWebAppFactory = new CustomWebApplicationFactory(connectionString);
        client = _testWebAppFactory.CreateClient();
        var token = GetAuth0AccessTokenAsync(auth0Configuration)
        .GetAwaiter()
        .GetResult();
        client
        .DefaultRequestHeaders
        .TryAddWithoutValidation(
        "Authorization",
        $"bearer {token}");
        Environment
        .SetEnvironmentVariable(
        "ConnectionStrings:DefaultConnection",
        connectionString);
        databaseContext = new QimiaSchoolDbContextFactory()
        .CreateDbContext(new[] { connectionString });
    }
    private static Auth0Configuration GetAuth0Configuration()
    {
        var configBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.Development.json")
        .AddEnvironmentVariables();
        var configuration = configBuilder.Build();
        var auth0Configuration = new Auth0Configuration();
        configuration.GetSection("Auth0").Bind(auth0Configuration);
        return auth0Configuration;
    }
    private static async Task<string?> GetAuth0AccessTokenAsync(Auth0Configuration auth0Configuration)
    {
        

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://dev-gl4uz4rx8o7tfjtp.us.auth0.com");
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var requestData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", "WziaVsphTGGW9Ntu28fdHGLdMPdTWue0"),
            new KeyValuePair<string, string>("client_secret", "BfMVwjhJh-NGRhx4goX59z3FFgnDrjzHO6U5caXrOi6GEs0saq5lQqCbpeSomVvi"),
            new KeyValuePair<string, string>("audience", "https://dev-gl4uz4rx8o7tfjtp.us.auth0.com/api/v2/")
        });
        var response = await httpClient.PostAsync("/oauth/token", requestData);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error while requesting the Auth0 access token.");
        }
        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
        return tokenResponse?.AccessToken;
    }
    public void Dispose()
    {
        _testWebAppFactory?.Dispose();
        databaseContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}