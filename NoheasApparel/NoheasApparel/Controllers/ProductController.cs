using Microsoft.AspNetCore.Mvc;
using NoheasApparel;
using NoheasApparel.ViewModels;
using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.Models;
using NoheasApparel.Models.Models;
using NoheasApparel.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using NoheasApparel.Utility;

namespace NoheasApparel.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork ctx)
        { 
            _unitOfWork = ctx;
        }

        [Route("products/")]
        public async Task<IActionResult> Index(string searchInput, int pageNum, string brandName, string categoryName, string genderName)
        {
            //GETS ALL PRODUCTS
            var products = _unitOfWork.Products.GetAllAsync();
            ProductViewModel productVM = new ProductViewModel()
            {
                //get all category for category drop down button
                Categories = _unitOfWork.Categories.GetAll().Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryID.ToString(),
                }),
                //get all brands for brands drop down button
                Brands = _unitOfWork.Brands.GetAll().Select(x => new SelectListItem
                {
                    Text = x.BrandName,
                    Value = x.BrandID.ToString(),
                }),
                //get all Genders for gender drop down button
                Genders = _unitOfWork.Genders.GetAll().Select(x => new SelectListItem
                {
                    Text = x.GenderName,
                    Value = x.GenderID.ToString(),
                })
            };

            if (string.IsNullOrEmpty(brandName))
            {
                ViewData["BrandName"] = "";
            }
            else
            {
                ViewData["BrandName"] = brandName;
            }

            if (string.IsNullOrEmpty(categoryName))
            {
                ViewData["CategoryName"] = "";
            }
            else
            {
                ViewData["CategoryName"] = categoryName;
            }
            if (string.IsNullOrEmpty(genderName))
            {
                ViewData["GenderName"] = "";
            }
            else
            {
                ViewData["GenderName"] = genderName;
            }

            if (!string.IsNullOrEmpty(searchInput))
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(searchInput.ToLower()) || p.ProductDescription.ToLower().Contains(searchInput.ToLower())
                                        || p.Gender.ToLower().Contains(searchInput.ToLower()) || p.Category.ToLower().Contains(searchInput.ToLower()));

            }
            if (!string.IsNullOrEmpty(brandName))
            {
                products = products.Where(x => x.Brand.ToLower() == brandName.ToLower());
            }
            if (!string.IsNullOrEmpty(categoryName))
            {
                products = products.Where(x => x.Category.ToLower() == categoryName.ToLower());
            }
            if (!string.IsNullOrEmpty(genderName))
            {
                products = products.Where(x => x.Gender.ToLower() == genderName.ToLower());
            }

            if (pageNum < 1)
            {
                pageNum = 1;
            }
            ViewData["SearchInput"] = searchInput;
            int pageSize = 5;
            return View(await PagedList<ProductViewModel>.CreateAsync(products, pageNum, 8, productVM));
        }


        //get action of details
        [HttpGet]
        public IActionResult Details(int id) 
        {
          
            var product = _unitOfWork.Products.Find(x => x.ProductID == id, includeProperties: "Brand,Category,Gender");

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                ProductID = product.ProductID,
                Product = product
            };

            return View("Details", shoppingCart);
        }


        //post action method if a user add product to cart
        //must be authorized user/signed in
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.CartID = 0;

            //user identifier
            var userIdentityClaim = (ClaimsIdentity)User.Identity;
            //gets the id of the log in user
            var claim = userIdentityClaim?.FindFirst(ClaimTypes.NameIdentifier);

            // if model state is valid, add product to cart.
            if (ModelState.IsValid)
            {
                
                // store the logged in user ID to shopping cart user id parameter
                shoppingCart.NoheasApparelUserID = claim.Value;

                //gets the shopping cart from the DB based on the logged in USER ID
                ShoppingCart cartDB = _unitOfWork.ShoppingCarts.Find(x => x.NoheasApparelUserID == shoppingCart.NoheasApparelUserID
                                                                    && x.ProductID == shoppingCart.ProductID, includeProperties: "Product");

                //checks if there is found cart from DB of the logged in user.
                if (cartDB == null)
                {
                    //if null or if logged in user has no existing product in cart from DB, make/add one 
                    _unitOfWork.ShoppingCarts.Add(shoppingCart);
                }
                //else if logged in user has cart, add the count of the product
                else 
                {
                    cartDB.Count += shoppingCart.Count;
                    _unitOfWork.ShoppingCarts.Update(cartDB);
                }
                _unitOfWork.Save();

                TempData["success"] = "Product Added to Cart Successfully.";

                //updates the count of the user cart
                var cnt = _unitOfWork.ShoppingCarts.GetAll().Where(x => x.NoheasApparelUserID == shoppingCart.NoheasApparelUserID).ToList().Count();
                HttpContext.Session.SetInt32(StaticDetail.SessionShopCart, cnt);
                return RedirectToAction("Index");
            }
            //else if model state is not valid return back to view with the product that is passed in
            else 
            {
                var product = _unitOfWork.Products.Find(x => x.ProductID == shoppingCart.ProductID, includeProperties: "Brand,Category,Gender");

                ShoppingCart cart = new ShoppingCart()
                {
                    ProductID = product.ProductID,
                    Product = product,
                };

                return View(cart);
            }
        }


        
       
    }
}
