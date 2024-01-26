using NoheasApparel.DataAccess.SeedData;
using NoheasApparel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoheasApparel.Models.Models;

namespace NoheasApparel.DataAccess
{
    public class NoheasApparelContext : IdentityDbContext<IdentityUser>
    {
        public NoheasApparelContext(DbContextOptions<NoheasApparelContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Brand> Brands { get; set; }
       

        public DbSet<Category> Categories { get; set; }
        public DbSet<NoheasApparelUser> NoheasApparelUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new SeedProduct());
            modelBuilder.ApplyConfiguration(new SeedGender());
            modelBuilder.ApplyConfiguration(new SeedBrand());
            modelBuilder.ApplyConfiguration(new SeedCategory());
        }
    }
}
