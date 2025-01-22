using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FooBar.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FooBar.Tests.Controllers;

[TestFixture] 
public class ExternalApiControllerIntegrationTest
{
    private HttpClient client;
    private WebApplicationFactory<Program> application;
    
    [SetUp]
    public void SetUp()
    {
        // Setup the fake post data to be returned
        var fakePost = new
        {
            Id = 618,
            Title = "Test Title",
            UserId = 382,
            Body = "Test Body"
        };

        // Create the WebApplicationFactory with the correct configuration
        application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the default HttpClient registration to avoid conflicts
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IHttpClientFactory));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor); // Remove the default IHttpClientFactory registration
                    }

                    // Mock HttpMessageHandler to simulate HTTP responses
                    var mockHandler = new Mock<HttpMessageHandler>();

                    // Setup the mock to return a custom response
                    mockHandler.Protected()
                        .Setup<Task<HttpResponseMessage>>("SendAsync", 
                            It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>())
                        .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(
                                JsonConvert.SerializeObject(fakePost), Encoding.UTF8, "application/json")
                        });

                    // Create HttpClient using the mocked handler
                    var mockHttpClient = new HttpClient(mockHandler.Object);

                    // Register the mocked HttpClient into the service collection
                    services.AddSingleton(mockHttpClient);
                });
            });

        client = application.CreateClient();
    }
    
    [Test]
    public async Task GetPostByIdWorksAsExpected()
    {
        /*await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();*/
 
        var response = await client.GetAsync("api/ExternalApi/posts/1");
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

        string responseMessage = messageElement.GetString(); 
        int responseId = idElement.GetInt32(); 
        int responsePostId = postIdElement.GetInt32(); 
        Assert.That(responseMessage, Is.EqualTo("Post 1 retrieved successfully."), "The response message is incorrect.");
        Assert.That(responseId, Is.EqualTo(1), "The idElement is incorrect.");
        Assert.That(responsePostId, Is.EqualTo(1), "The postIdElement is incorrect.");
        /*Assert.Equal(200, (int)response.StatusCode, "The status code should be 200 OK.");
        // Assert(response.StatusCode).Should().Be(HttpStatusCode.OK);
        Assert.AreEqual(1, response.Id, "The post ID should be 1.");
        string responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine(responseBody);
        responseBody.Should().Be("{\"version\":\"0.0.0.0\"}");*/
            
    } 
}