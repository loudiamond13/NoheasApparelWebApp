using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel;
using Microsoft.EntityFrameworkCore;
using NoheasApparel.Models;

namespace NoheasApparel.DataAccess.Repository
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
       

       private readonly NoheasApparelContext _contextDB;

        public BrandRepository(NoheasApparelContext ctx) 
            : base(ctx) => _contextDB = ctx; 

        public void Update(Brand brand) 
        {
            _contextDB.Brands.Update(brand);
        }

     
    }
}
