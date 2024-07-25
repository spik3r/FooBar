// Repositories/ISampleEntityRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FooBar.Models;

namespace FooBar.Repositories
{
    public interface ISampleEntityRepository
    {
        Task<IEnumerable<SampleEntity>> GetAllAsync();
        Task<SampleEntity> GetByIdAsync(int id);
        Task<SampleEntity> AddAsync(SampleEntity entity);
        Task<SampleEntity> UpdateAsync(SampleEntity entity);
        Task DeleteAsync(int id);
    }
}

