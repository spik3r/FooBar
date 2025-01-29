using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace FooBar.Tests.Controllers;

[TestFixture] 
public class ExternalApiControllerIntegrationTest
{
    
    private HttpClient client;
    private WebApplicationFactory<Program> application;
    private WireMockServer wireMockServer; 
    
    [SetUp]
    public void SetUp()
    {
        
        // application = new WebApplicationFactory<Program>();
        application = new WebApplicationFactory<Program>() 
            .WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Remove default appsettings.json file
                config.Sources.Clear();

                // Add the custom test appsettings file
                config.AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true);
            });
        });
        client = application.CreateClient();
        
        wireMockServer = WireMockServer.Start(new WireMockServerSettings
        {
            Urls = ["http://localhost:8888"]
        });
        Console.WriteLine($"WireMock server running at {wireMockServer.Url}");
        
        // Mock the response for the JSON Placeholder API
        wireMockServer.Given(Request.Create().WithPath("/posts/2").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody("{ \"userId\": 123, \"id\": 2, \"title\": \"mocked title\", \"body\": \"mocked body content\" }"));
        

    }

    [TearDown]
    public void TearDown()
    {
        wireMockServer.Stop();
        application.Dispose();
    }
    
    [Test]
    public async Task GetPostByIdWorksAsExpectedCallingRealApi()
    {
        await using var realApplication = new WebApplicationFactory<Program>();
        using var realClient = realApplication.CreateClient();
 
        var response = await realClient.GetAsync("api/ExternalApi/posts/1");
        Assert.That(200 == (int)response.StatusCode, "The status code should be 200 OK.");
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
        Assert.That(!string.IsNullOrWhiteSpace(responseBody), "The response body should not be null or empty.");
        using JsonDocument doc = JsonDocument.Parse(responseBody);
        JsonElement root = doc.RootElement;
        JsonElement responseModelElement = root.GetProperty("responseModel");
        JsonElement messageElement = root.GetProperty("message"); 
        JsonElement idElement = responseModelElement.GetProperty("id"); 
        JsonElement postElement = responseModelElement.GetProperty("post"); 
        JsonElement postIdElement = postElement.GetProperty("id");
        JsonElement postTitleElement = postElement.GetProperty("title");

        string responseMessage = messageElement.GetString(); 
        int responseId = idElement.GetInt32(); 
        int responsePostId = postIdElement.GetInt32(); 
        string responseTitle = postTitleElement.GetString();
        Assert.That(responseMessage, Is.EqualTo("Post 1 retrieved successfully."), "The response message is incorrect.");
        Assert.That(responseId, Is.EqualTo(1), "The idElement is incorrect.");
        Assert.That(responsePostId, Is.EqualTo(1), "The postIdElement is incorrect.");
        Assert.That(responseTitle, Is.EqualTo("sunt aut facere repellat provident occaecati excepturi optio reprehenderit"), "The responseTitle is incorrect.");
    } 
    
    [Test]
    public async Task GetPostById2WorksAsExpectedCallingWireMockApi()
    {
        var response = await client.GetAsync("api/ExternalApi/posts/2");
        Assert.That(200 == (int)response.StatusCode, "The status code should be 200 OK.");
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
        Assert.That(!string.IsNullOrWhiteSpace(responseBody), "The response body should not be null or empty.");
        using JsonDocument doc = JsonDocument.Parse(responseBody);
        JsonElement root = doc.RootElement;
        JsonElement responseModelElement = root.GetProperty("responseModel");
        JsonElement messageElement = root.GetProperty("message"); 
        JsonElement idElement = responseModelElement.GetProperty("id"); 
        JsonElement postElement = responseModelElement.GetProperty("post"); 
        JsonElement postIdElement = postElement.GetProperty("id");
        JsonElement postTitleElement = postElement.GetProperty("title");

        string responseMessage = messageElement.GetString(); 
        int responseId = idElement.GetInt32(); 
        int responsePostId = postIdElement.GetInt32(); 
        string responseTitle = postTitleElement.GetString();
        Assert.That(responseMessage, Is.EqualTo("Post 2 retrieved successfully."), "The response message is incorrect.");
        Assert.That(responseId, Is.EqualTo(2), "The idElement is incorrect.");
        Assert.That(responsePostId, Is.EqualTo(2), "The postIdElement is incorrect.");
        Assert.That(responseTitle, Is.EqualTo("mocked title"), "The responseTitle is incorrect.");
    } 
}