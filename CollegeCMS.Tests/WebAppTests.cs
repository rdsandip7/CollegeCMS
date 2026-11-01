using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CollegeCMS.Tests;

/// <summary>Boots the real web app against a throwaway SQLite file and drives it over HTTP.</summary>
public partial class WebAppTests : IClassFixture<WebAppTests.AppFactory>
{
    private readonly AppFactory _factory;

    public WebAppTests(AppFactory factory) => _factory = factory;

    [Fact]
    public async Task Landing_page_is_public()
    {
        var response = await _factory.CreateClient().GetAsync("/");

        response.EnsureSuccessStatusCode();
        Assert.Contains("Engineering and IT education", await response.Content.ReadAsStringAsync());
    }

    [Theory]
    [InlineData("/Student")]
    [InlineData("/Employee")]
    [InlineData("/Course")]
    [InlineData("/Department")]
    [InlineData("/Level")]
    [InlineData("/Home/Dashboard")]
    public async Task Management_pages_require_sign_in(string url)
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.StartsWith("/User/Login", response.Headers.Location!.PathAndQuery);
    }

    [Fact]
    public async Task Admin_can_sign_in_and_browse_every_list()
    {
        var client = _factory.CreateClient();

        var loginPage = await client.GetStringAsync("/User/Login");
        var token = AntiforgeryToken().Match(loginPage).Groups[1].Value;
        var login = await client.PostAsync("/User/Login", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["UserName"] = "admin",
            ["Password"] = "Admin@123",
            ["__RequestVerificationToken"] = token,
        }));

        login.EnsureSuccessStatusCode();
        Assert.Contains("Welcome back, admin", await login.Content.ReadAsStringAsync());

        foreach (var url in new[] { "/Home/Dashboard", "/Student?page=20", "/Student", "/Employee", "/Course", "/Department", "/Level", $"/Student/Details/STU-{CurrentIntake}-0001", "/Course?sort=department_desc&search=eng" })
        {
            var response = await client.GetAsync(url);
            Assert.True(response.IsSuccessStatusCode, $"{url} returned {(int)response.StatusCode}");
        }
    }

    [Fact]
    public async Task Unknown_record_returns_404()
    {
        var response = await _factory.CreateClient().GetAsync("/does-not-exist");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static int CurrentIntake => DateTime.Today.Month >= 2 ? DateTime.Today.Year : DateTime.Today.Year - 1;

    [GeneratedRegex("name=\"__RequestVerificationToken\" type=\"hidden\" value=\"([^\"]+)\"")]
    private static partial Regex AntiforgeryToken();

    public sealed class AppFactory : WebApplicationFactory<Program>
    {
        private readonly string _databasePath = Path.Combine(Path.GetTempPath(), $"collegecms-test-{Guid.NewGuid():N}.db");

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.UseSetting("ConnectionStrings:CollegeCMS", $"Data Source={_databasePath};Pooling=False");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            File.Delete(_databasePath);
        }
    }
}
