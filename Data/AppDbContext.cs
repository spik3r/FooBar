using Microsoft.EntityFrameworkCore;
using FooBar.Models;

namespace FooBar.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SampleEntity> SampleEntities { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure additional model options or relationships here if needed
        }
    }
}

