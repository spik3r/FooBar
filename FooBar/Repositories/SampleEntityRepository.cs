// Repositories/SampleEntityRepository.cs
using Microsoft.EntityFrameworkCore;
using FooBar.Data;
using FooBar.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooBar.Repositories
{
    public class SampleEntityRepository : ISampleEntityRepository
    {
        private readonly AppDbContext _context;

        public SampleEntityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SampleEntity>> GetAllAsync()
        {
            return await _context.SampleEntities.ToListAsync();
        }

        public async Task<SampleEntity> GetByIdAsync(int id)
        {
            return await _context.SampleEntities.FindAsync(id);
        }

        public async Task<SampleEntity> AddAsync(SampleEntity entity)
        {
            _context.SampleEntities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<SampleEntity> UpdateAsync(SampleEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.SampleEntities.FindAsync(id);
            if (entity != null)
            {
                _context.SampleEntities.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

