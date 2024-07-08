using Microsoft.EntityFrameworkCore;
using VideoManagerApi.Models;

namespace VideoManagerApi.Data
{
    public class ProductVideoContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Video> Videos { get; set; }

        public DbSet<User> Users { get; set; }

        public ProductVideoContext(DbContextOptions<ProductVideoContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
