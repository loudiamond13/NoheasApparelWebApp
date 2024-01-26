using NoheasApparel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace NoheasApparel.DataAccess.SeedData
{
    internal class SeedGender : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> entity)
        {
            entity.HasData(
                new Gender
                {
                    GenderID = 1,
                    GenderName = "Men"
                },
                 new Gender
                 {
                     GenderID = 2,
                     GenderName = "Women"
                 },
                  new Gender
                  {
                      GenderID = 3,
                      GenderName = "Unisex"
                  }

                );
        }
    }
}
