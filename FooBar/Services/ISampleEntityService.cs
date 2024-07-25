// Services/ISampleEntityService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FooBar.Models;

namespace FooBar.Services
{
    public interface ISampleEntityService
    {
        Task<IEnumerable<SampleEntity>> GetAllAsync();
        Task<SampleEntity> GetByIdAsync(int id);
        Task<SampleEntity> AddAsync(SampleEntity entity);
        Task<SampleEntity> UpdateAsync(SampleEntity entity);
        Task DeleteAsync(int id);
    }
}

