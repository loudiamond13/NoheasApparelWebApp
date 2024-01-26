using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoheasApparel.Utility;
using NoheasApparel.Models;
using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.Models.ViewModels;

namespace NoheasApparel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetail.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork x)
        {
            _unitOfWork = x;
        }

        public IActionResult Index()
        {
            //gets all categories
            var categories = _unitOfWork.Categories.GetAll().OrderBy(x => x.CategoryID).ToList();
            //returns all categories
            return View(categories);
        }

        [HttpGet]
        public IActionResult AddEdit(int? id)
        {

            //if id is == 0, create/add product
            if ((id == 0) || id == null)
            {
                ViewBag.Action = "Add";
                Category category = new Category();
                return View("AddEdit", category);
            }
            // else if id has value, update the product
            else
            {
                //find the passed in id from the categoryDB
                Category category = _unitOfWork.Categories.Find(x => x.CategoryID == id);
                ViewBag.Action = "Edit";
                //return found category
                return View("AddEdit", category);
            }

        }





        [HttpPost]
        public IActionResult AddEdit(Category category)
        {
            //check if input is valid 
            if (ModelState.IsValid)
            {
                //if id is == 0, create/add product
                if ((category.CategoryID == 0) || category.CategoryID == null)
                {
                    //add the category into the DB
                    _unitOfWork.Categories.Add(category);
                    TempData["success"] = "Category added successfully.";
                    
                }
                //else if the categoryID has value, update the passed in category
                else
                {
                    _unitOfWork.Categories.Update(category);
                    TempData["success"] = "Category updated successfully.";

                }
                _unitOfWork.Save();
                return RedirectToAction("Index", "Category");
            }
            else
            {
                var categories = _unitOfWork.Categories.GetAll().ToList();
                return View("AddEdit", category);
             }
        }

        
        
        //delete acton category
        [HttpGet]
        public IActionResult Delete(int? id) 
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel()
            {
                Category = _unitOfWork.Categories.Find(x => x.CategoryID == id),
                Products = _unitOfWork.Products.GetAll(includeProperties: "Category").Where(x=>x.CategoryID == id).ToList()
            };

            //delete the category
            return View(categoryViewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            TempData["success"] = "Category successfully deleted";
            Category category = _unitOfWork.Categories.Find(x => x.CategoryID == id);
            _unitOfWork.Categories.Delete(category);
            //save DB
            _unitOfWork.Save();
            //return to index action
            return RedirectToAction("Index");
        }
    }
}
