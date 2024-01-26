using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.DataAccessLibrary.DataAccess.Repository.Interfaces;
using NoheasApparel.Models;
using NoheasApparel.Models.Models;

namespace NoheasApparel.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly NoheasApparelContext _contextDB;
        public ShoppingCartRepository(NoheasApparelContext ctx)
                : base(ctx) => _contextDB = ctx;

        public void Update(ShoppingCart ShoppingCart) 
        {
            _contextDB.ShoppingCarts.Update(ShoppingCart);
        }

     

        

    }





}
