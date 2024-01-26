using NoheasApparel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NoheasApparel.DataAccess.SeedData
{
    public class SeedCategory : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.HasData(
                new Category
                {
                    CategoryID = 1,
                    CategoryName = "Shoes"
                },
                new Category
                {
                    CategoryID = 2,
                    CategoryName = "Hoodie"
                },
                new Category
                {
                    CategoryID = 3,
                    CategoryName = "Slides"
                },
                new Category
                {
                    CategoryID = 4,
                    CategoryName = "Cap"
                });
        }
    }
}
