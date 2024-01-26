using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoheasApparel.Utility;
using NoheasApparel.Models;
using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.Models.ViewModels;
using NoheasApparel.DataAccess.Repository;
using NoheasApparel.DataAccess;
using Microsoft.EntityFrameworkCore;
using NoheasApparel.Models.Models;

namespace NoheasApparel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetail.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly NoheasApparelContext _noheasContext;

        public UserController(IUnitOfWork x, NoheasApparelContext ctx)
        {
            _unitOfWork = x;
            _noheasContext = ctx;
        }

        public IActionResult Index() 
        {
            return View();
        }

        #region api calls
        [HttpGet]
        public IActionResult GetUsers()
        {
            //gets all user
            var users = _unitOfWork.NoheasApparelUsers.GetAll().ToList();


            var asd = _noheasContext.NoheasApparelUsers.ToList();
            var userRole = _noheasContext.UserRoles.ToList();
            var roles = _noheasContext.Roles.ToList();

            foreach (var user in asd) 
            {
                var roleID = userRole.FirstOrDefault(x => x.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(x => x.Id == roleID).Name;
                //if (user.Company == null)
                //{
                //    user.Company = new Company()
                //    {
                //        Name = ""
                //    };
                //}
            }

            //returns all user
            return Json(new { data = asd });
        }

        [HttpPost]
        public IActionResult UnlockLock([FromBody] string id)
        {
            var user = _noheasContext.NoheasApparelUsers.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return Json(new { success = false, message = "Error while unlocking/locking user" });
            }

            // if user is currently lock
            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                // user is currently lock
                user.LockoutEnd = DateTime.Now;
            }
            //else if user is not lock, 
            //lock the user
            else 
            {
                user.LockoutEnd = DateTime.Now.AddYears(1);
            }
            _noheasContext.SaveChanges();
            return Json(new { success = true, message = "Locking/Unlocking successful." });
        }
        #endregion

    }
}


//[HttpGet]
//public IActionResult AddEdit(int? id)
//{

//    //if id is == 0, create/add product
//    if ((id == 0) || id == null)
//    {
//        ViewBag.Action = "Add";
//        User User = new User();
//        return View("AddEdit", User);
//    }
//    // else if id has value, update the product
//    else
//    {
//        //find the passed in id from the UserDB
//        User User = _unitOfWork.Categories.Find(x => x.UserID == id);
//        ViewBag.Action = "Edit";
//        //return found User
//        return View("AddEdit", User);
//    }

//}





//[HttpPost]
//public IActionResult AddEdit(User User)
//{
//    //check if input is valid 
//    if (ModelState.IsValid)
//    {
//        //if id is == 0, create/add product
//        if ((User.UserID == 0) || User.UserID == null)
//        {
//            //add the User into the DB
//            _unitOfWork.Categories.Add(User);
//            TempData["success"] = "User added successfully.";

//        }
//        //else if the UserID has value, update the passed in User
//        else
//        {
//            _unitOfWork.Categories.Update(User);
//            TempData["success"] = "User updated successfully.";

//        }
//        _unitOfWork.Save();
//        return RedirectToAction("Index", "User");
//    }
//    else
//    {
//        var categories = _unitOfWork.Categories.GetAll().ToList();
//        return View("AddEdit", User);
//    }
//}



////delete acton User
//[HttpGet]
//public IActionResult Delete(int? id)
//{
//    UserViewModel UserViewModel = new UserViewModel()
//    {
//        User = _unitOfWork.Categories.Find(x => x.UserID == id),
//        Products = _unitOfWork.Products.GetAll(includeProperties: "User").Where(x => x.UserID == id).ToList()
//    };

//    //delete the User
//    return View(UserViewModel);
//}

//[HttpPost]
//public IActionResult Delete(int id)
//{
//    TempData["success"] = "User successfully deleted";
//    User User = _unitOfWork.Categories.Find(x => x.UserID == id);
//    _unitOfWork.Categories.Delete(User);
//    //save DB
//    _unitOfWork.Save();
//    //return to index action
//    return RedirectToAction("Index");
//}