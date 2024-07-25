
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FooBar.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace FooBar.Services
{
    public class JsonPlaceholderService : IJsonPlaceholderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<JsonPlaceholderService> _logger;

        public JsonPlaceholderService(IHttpClientFactory httpClientFactory, ILogger<JsonPlaceholderService> logger)
        // public JsonPlaceholderService(HttpClient httpClient, ILogger<JsonPlaceholderService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
            // _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.LogInformation("JsonPlaceholderService instantiated.");
            _logger.LogInformation("Base address: {BaseAddress}", _httpClient.BaseAddress);
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching posts from {Url}", _httpClient.BaseAddress + "posts");

                var response = await _httpClient.GetAsync("posts");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<List<Post>>(json);

                _logger.LogInformation("Posts retrieved: {Posts}", JsonConvert.SerializeObject(posts));

                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching posts from JsonPlaceholder API.");
                throw;
            }
        }

        public string GetBase()
        {
            if (_httpClient.BaseAddress != null)
            {
                _logger.LogInformation("GetBase this: {BaseAddress}", _httpClient.BaseAddress);
                return _httpClient.BaseAddress.ToString();
            }
            else
            {
                _logger.LogWarning("HttpClient BaseAddress is null.");
                return "BaseAddress not set"; // Or throw an exception, depending on your error handling strategy
            }
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching post with Id {Id} from {Url}", id, _httpClient.BaseAddress + $"posts/{id}");

                var response = await _httpClient.GetAsync($"posts/{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var post = JsonConvert.DeserializeObject<Post>(json);

                _logger.LogInformation("Post retrieved: {@Post}", post);

                return post;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching post from JsonPlaceholder API.");
                throw;
            }
        }
    }
}

