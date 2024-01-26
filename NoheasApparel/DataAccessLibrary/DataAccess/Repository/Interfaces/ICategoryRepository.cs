using NoheasApparel.Models;

namespace NoheasApparel.DataAccess.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {

        void Update(Category category);
       
    }
}
