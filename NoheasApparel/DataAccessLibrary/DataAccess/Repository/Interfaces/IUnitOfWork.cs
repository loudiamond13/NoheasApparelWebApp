using NoheasApparel.DataAccessLibrary.DataAccess.Repository.Interfaces;

namespace NoheasApparel.DataAccess.Repository.Interfaces
{
    public interface IUnitOfWork 
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IBrandRepository Brands { get; }
        IGenderRepository Genders { get; }
        INoheasApparelUserRepository NoheasApparelUsers { get; }
        IShoppingCartRepository ShoppingCarts { get; }
        IOrderDetailRepository OrderDetails { get; }
        IOrderHeaderRepository OrderHeaders { get; }

       
        void Save();
    }
}
