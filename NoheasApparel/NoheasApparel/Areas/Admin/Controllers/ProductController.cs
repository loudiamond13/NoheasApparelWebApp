using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoheasApparel.Utility;
using NoheasApparel.ViewModels;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NoheasApparel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetail.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork x, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = x;
        }

        //index view of admin/product
        public ViewResult Index()
        {
            //returns all product ordered by product ID
            IEnumerable<Product> products = _unitOfWork.Products
                                                       .GetAll(includeProperties:"Category,Brand,Gender")
                                                        .OrderBy(prod => prod.ProductID).ToList();
           
            //return to the area/page of admin products
            return View("Index", products);
        }


      



        //get action of add/edit
        [HttpGet]
        public IActionResult AddEdit(int? id) 
        {
            //if id is equal to zero, add new product
            //else edit product
            if (id == 0 || id == null)
            {
                ViewBag.Action = "Add";


                //store all categories,brands,genders into the viewmodel lists
                ProductViewModel productVM = new ProductViewModel()
                {
                    //creates new product and store into the productVM.Product
                    Product = new Product(),

                    // get all genders and store in a selectlist
                    Genders = _unitOfWork.Genders.GetAll().Select(x => new SelectListItem
                    {
                        Text = x.GenderName,
                        Value = x.GenderID.ToString(),
                    }),
                    // get all category and store in a selectlist
                    Categories = _unitOfWork.Categories.GetAll().Select(x => new SelectListItem
                    {
                        Text = x.CategoryName,
                        Value = x.CategoryID.ToString()
                    }),
                    // get all brand and store in a selectlist
                    Brands = _unitOfWork.Brands.GetAll().Select(x => new SelectListItem
                    {
                        Text = x.BrandName,
                        Value = x.BrandID.ToString(),
                    }),
                };
                //returns to AddEdit POST with new product 
                return View("AddEdit", productVM);
            }
            // if if id > 0, find the product from the DB and edit/update it.
            else 
            {
                ViewBag.Action = "Edit";

                //find the passed in ProductID in DB
                Product product = _unitOfWork.Products.Find(prod => prod.ProductID == id);

                //store all categories,brands,genders into viewbag
                ProductViewModel productVM = new ProductViewModel()
                {
                    //stores the product 
                    Product = product,

                    // get all genders and store in a selectlist
                    Genders = _unitOfWork.Genders.GetAll().Select(x => new SelectListItem
                    {
                        Text = x.GenderName,
                        Value = x.GenderID.ToString(),
                    }),
                    // get all category and store in a selectlist
                    Categories = _unitOfWork.Categories.GetAll().Select(x => new SelectListItem
                    {
                        Text = x.CategoryName,
                        Value = x.CategoryID.ToString()
                    }),
                    // get all brand and store in a selectlist
                    Brands = _unitOfWork.Brands.GetAll().Select(x => new SelectListItem
                    {
                        Text = x.BrandName,
                        Value = x.BrandID.ToString(),
                    }),
                };
                //return the product to POST
                return View("AddEdit", productVM);
            }

        }


        //post action of add/edit
        [HttpPost]
        public ActionResult AddEdit(ProductViewModel passedInProductVM, IFormFile? productIMG) 
        {
            


            if (ModelState.IsValid) // checks if all entry ara valid
            {
                string wwwRootpath = _webHostEnvironment.WebRootPath;

                if (productIMG != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(productIMG.FileName);
                    string productPath = Path.Combine(wwwRootpath, @"content\images\product");

                    if (!string.IsNullOrEmpty(passedInProductVM.Product.ProductImageURL))
                    {
                        var oldURL = Path.Combine(
                                                wwwRootpath, passedInProductVM.Product.ProductImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldURL))
                        {
                            System.IO.File.Delete(oldURL);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        productIMG.CopyTo(fileStream);
                    }

                    passedInProductVM.Product.ProductImageURL = @"~/content/images/product/" + fileName;
                }
       


                // if the passed in product has no ID or product ID is equal to zero
                // add to product to DB
                if (passedInProductVM.Product.ProductID == 0)
                {
                    //adds the product into the DB
                    _unitOfWork.Products.Add(passedInProductVM.Product);
                    TempData["success"] = "Product added successfully!";                
                }
                //else if the passed in product has ID, update the passed in product in DB
                else
                {
                    //updates the product
                    _unitOfWork.Products.Update(passedInProductVM.Product);
                    TempData["success"] = "Product updated successfully!";
                }

                //save changes
                _unitOfWork.Save();
                //redirect to index
                return RedirectToAction("Index", "Product");
            }
            //else if the inputs are not valid
            else 
            {
                //store all categories,brands,genders into viewbag
                ProductViewModel productVM = new ProductViewModel()
                {

                    Product = passedInProductVM.Product,

                    // get all genders and store in a selectlist
                    Genders = _unitOfWork.Genders.GetAll().Select(x => new SelectListItem
                    {
                        Text = x.GenderName,
                        Value = x.GenderID.ToString(),
                    }),
                    // get all category and store in a selectlist
                    Categories = _unitOfWork.Categories.GetAll().Select(x => new SelectListItem
                    {
                        Text = x.CategoryName,
                        Value = x.CategoryID.ToString()
                    }),
                    // get all brand and store in a selectlist
                    Brands = _unitOfWork.Brands.GetAll().Select(x => new SelectListItem
                    {
                        Text = x.BrandName,
                        Value = x.BrandID.ToString(),
                    }),
                };
                return View(productVM);

            }
        }




        #region api calls
        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = _unitOfWork.Products.GetAll(includeProperties: "Category,Brand,Gender").ToList();
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id) 
        {
            Product product = _unitOfWork.Products.Find(x => x.ProductID == id);

            if (product == null) 
            {
                TempData["error"] = "Product deletion was unsuccessful.";
                return Json(new { success = false });
            }

            if (product.ProductImageURL != null)
            {
                var imageToBeDeleted = Path.Combine(_webHostEnvironment.WebRootPath, product.ProductImageURL.TrimStart('\\'));



                if (System.IO.File.Exists(imageToBeDeleted))
                {
                    System.IO.File.Delete(imageToBeDeleted);
                }
            }
           
                TempData["success"] = "Product successfuly deleted.";
                _unitOfWork.Products.Delete(product);
                _unitOfWork.Save();
            

            

            
            return Json(new { success = true, message = "Success"});
        }

        #endregion
    }
}
