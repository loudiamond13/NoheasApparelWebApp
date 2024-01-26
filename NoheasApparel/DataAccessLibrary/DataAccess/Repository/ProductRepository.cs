using Microsoft.EntityFrameworkCore;
using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.Models;
using NoheasApparel.Models.ViewModels;
using NoheasApparel.ViewModels;

namespace NoheasApparel.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        private readonly NoheasApparelContext _contextDB;
        public ProductRepository(NoheasApparelContext ctx)
                : base(ctx) => _contextDB = ctx;

        public void Update(Product product)
        {
            Product productToBeUpdated = _contextDB.Products.FirstOrDefault(x => x.ProductID == product.ProductID);

            if (product != null)
            { 
                productToBeUpdated.ProductName = product.ProductName;
                productToBeUpdated.ProductPrice = product.ProductPrice;
                productToBeUpdated.ProductColor = product.ProductColor;
                productToBeUpdated.BrandID = product.BrandID;
                productToBeUpdated.GenderID = product.GenderID;
                productToBeUpdated.CategoryID = product.CategoryID;
                productToBeUpdated.ProductDescription = product.ProductDescription;
                if (product.ProductImageURL != null)
                { 
                    productToBeUpdated.ProductImageURL = product.ProductImageURL;
                }

            }
        }

        public IQueryable<ProductViewModel> GetAllAsync() 
        {
            var products = _contextDB.Products.Include(x=>x.Category).Include(x=>x.Brand).Include(x=>x.Gender)
                .Select(p => new ProductViewModel
                { 
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Price = p.ProductPrice,
                Color = p.ProductColor,
                ProductDescription = p.ProductDescription,
                Category = p.Category.CategoryName,
                Brand = p.Brand.BrandName,
                Gender = p.Gender.GenderName,
                ImageURL = p.ProductImageURL
                

            });
            return products;
        }

    }
}
