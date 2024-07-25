using System.Threading.Tasks;
using FooBar.Models;

namespace FooBar.Services
{

    public interface IJsonPlaceholderService
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        string GetBase();
    }
}

