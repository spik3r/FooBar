using FooBar.Models;

namespace FooBar.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class ExternalApiController : ControllerBase
{
    private readonly HttpClient _httpClient;

    // Inject HttpClient via constructor
    public ExternalApiController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet("posts")]
    public async Task<IActionResult> GetPosts()
    {
        // The external API URL (JSONPlaceholder in this case)
        string apiUrl = "https://jsonplaceholder.typicode.com/posts";

        // Make the request to the external API
        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            // Read the JSON content as a string
            string jsonContent = await response.Content.ReadAsStringAsync();

            // Optionally deserialize the JSON if you need to manipulate the data
            var data = JsonConvert.DeserializeObject<Post[]>(jsonContent);

            // Return the data as part of your API response
            return Ok(new
            {
                Message = "Here is the data from JSONPlaceholder",
                Data = data
            });
        }
        else
        {
            return StatusCode((int)response.StatusCode, new { Error = "Failed to fetch data from external API" });
        }
    }
    
    // Get a single post by ID
    [HttpGet("posts/{id}")]
    public async Task<ActionResult<ResponseModel>> GetPostById(int id)
    {
        // The external API URL (JSONPlaceholder in this case)
        string apiUrl = $"https://jsonplaceholder.typicode.com/posts/{id}";

        // Make the request to the external API
        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            // Read the JSON content as a string
            string jsonContent = await response.Content.ReadAsStringAsync();

            // Optionally deserialize the JSON if you want to manipulate the data
            var post = JsonConvert.DeserializeObject<Post>(jsonContent);

            ResponseModel responseModel = new ResponseModel(id, "something important...", post);
            // Return the post data as part of your API response
            return Ok(new
            {
                Message = $"Post {id} retrieved successfully.",
                // Post = post
                ResponseModel = responseModel
            });
        }
        else
        {
            return NotFound(new { Error = $"Post with ID {id} not found." });
        }
    }
}
