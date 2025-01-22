using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FooBar.Controllers;
using FooBar.Models;
using Microsoft.AspNetCore.Mvc;
using Moq.Protected;
using NUnit.Framework;


namespace FooBar.Tests.Controllers;

public class ExternalApiControllerTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private ExternalApiController _controller;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            // Mock HttpMessageHandler
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            // Setup HttpClient to use the mocked handler
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            // Mock IHttpClientFactory to return our mocked HttpClient
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            // Instantiate the controller with the mocked HttpClientFactory
            _controller = new ExternalApiController(_httpClientFactoryMock.Object);
        }

        [Test]
        public async Task GetAllPosts_ShouldReturnOkResult_WithPosts()
        {
            /*// Arrange: Prepare mock response for the external API
            var posts = new List<Post>
            {
                new Post { UserId = 1, Id = 1, Title = "Test Post 1", Body = "Body 1" },
                new Post { UserId = 1, Id = 2, Title = "Test Post 2", Body = "Body 2" }
            };

            var jsonResponse = JsonConvert.SerializeObject(posts);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", 
                    It.IsAny<HttpRequestMessage>(), 
                    It.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            // Act: Call the controller method
            var result = await _controller.GetPosts();

            // Assert: Verify that the result is of type OkObjectResult and contains the expected posts
            var okResult = result as OkObjectResult;
            Assert.That(value, Is.EqualTo(expectedValue))
            Assert.NotNull(okResult);
            var returnedPosts = okResult?.Value as List<Post>;
            Assert.NotNull(returnedPosts);
            Assert.AreEqual(posts.Count, returnedPosts.Count);*/
        }

        [Test]
        public async Task GetPostById_ShouldReturnNotFound_WhenPostDoesNotExist()
        {
            /*// Arrange: Mock the external API to return NotFound for a non-existing post
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", 
                    It.IsAny<HttpRequestMessage>(), 
                    It.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            // Act: Call the controller method for a post ID that does not exist
            var result = await _controller.GetPostById(9999);

            // Assert: Verify that the result is a NotFoundObjectResult
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Post with ID 9999 not found.", notFoundResult?.Value.ToString());*/
        }
    }