using NoheasApparel.Models;
using NoheasApparel.Models.ViewModels;
using NoheasApparel.ViewModels;

namespace NoheasApparel.DataAccess.Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);

        IQueryable<ProductViewModel> GetAllAsync();
    }
}
