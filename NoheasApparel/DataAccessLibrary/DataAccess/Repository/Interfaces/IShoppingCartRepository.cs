using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.Models.Models;

namespace NoheasApparel.DataAccessLibrary.DataAccess.Repository.Interfaces
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCart);
    }
}