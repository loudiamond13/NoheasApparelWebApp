using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.DataAccessLibrary.DataAccess.Repository;
using NoheasApparel.DataAccessLibrary.DataAccess.Repository.Interfaces;

namespace NoheasApparel.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private  NoheasApparelContext _contextDB;

        public IProductRepository Products { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IBrandRepository Brands { get; private set; }
        public IGenderRepository Genders { get; private set; }

        public INoheasApparelUserRepository NoheasApparelUsers { get; private set; }

        public IShoppingCartRepository ShoppingCarts { get; private set; }
        public IOrderDetailRepository OrderDetails { get; private set; }
        public IOrderHeaderRepository OrderHeaders { get; private set; }

        public UnitOfWork(NoheasApparelContext ctx)
        {
            _contextDB = ctx;
            Products = new ProductRepository(_contextDB);
            Categories = new CategoryRepository(_contextDB);
            Brands = new BrandRepository(_contextDB);
            Genders = new GenderRepository(_contextDB);
            NoheasApparelUsers = new NoheasApparelUserRepository(_contextDB);
            ShoppingCarts = new ShoppingCartRepository(_contextDB);
            OrderDetails = new OrderDetailRepository(_contextDB);
            OrderHeaders = new OrderHeaderRepository(_contextDB);
        }

     

       

        public void Save()
        {
             _contextDB.SaveChanges();
        }

        

    }
}
