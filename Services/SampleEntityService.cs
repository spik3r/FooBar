// Services/SampleEntityService.cs
using FooBar.Models;
using FooBar.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooBar.Services
{
    public class SampleEntityService : ISampleEntityService
    {
        private readonly ISampleEntityRepository _repository;

        public SampleEntityService(ISampleEntityRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SampleEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<SampleEntity> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<SampleEntity> AddAsync(SampleEntity entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<SampleEntity> UpdateAsync(SampleEntity entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

