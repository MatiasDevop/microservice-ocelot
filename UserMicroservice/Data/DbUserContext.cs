using Microsoft.EntityFrameworkCore;
using UserMicroservice.Model;

namespace UserMicroservice.Data
{
    public class DbUserContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbUserContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<User> Users { get; set; }
    }
}
