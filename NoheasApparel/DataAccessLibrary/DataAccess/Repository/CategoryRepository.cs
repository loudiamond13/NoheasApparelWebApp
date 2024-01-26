using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.Models;

namespace NoheasApparel.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly NoheasApparelContext _contextDB;
        public CategoryRepository(NoheasApparelContext ctx)
                : base(ctx) => _contextDB = ctx;

        public void Update(Category category) 
        {
            _contextDB.Categories.Update(category);
        }

     

        

    }





}
