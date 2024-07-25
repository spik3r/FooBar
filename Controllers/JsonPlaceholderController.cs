using Microsoft.AspNetCore.Mvc;
using FooBar.Models;
using FooBar.Services;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FooBar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JsonPlaceholderController : ControllerBase
    {
        private readonly IJsonPlaceholderService _service;

        public JsonPlaceholderController(IJsonPlaceholderService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<Post>>> Get()
        {
            var posts = await _service.GetPostsAsync();
            return Ok(posts);
        }

        [HttpGet("posts/{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _service.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpGet("baseurl")]
        public async Task<ActionResult<string>> GetBase()
        {
            var baseUrl = _service.GetBase();
            return Ok(baseUrl);
        }
    }
}

