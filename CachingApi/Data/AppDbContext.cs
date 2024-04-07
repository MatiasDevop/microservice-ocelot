using CachingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CachingApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Driver> Drivers { get; set; }
    }
}
