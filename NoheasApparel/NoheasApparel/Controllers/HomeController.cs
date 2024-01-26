
using Microsoft.AspNetCore.Mvc;
using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.Utility;
using System.Diagnostics;
using System.Security.Claims;

namespace NoheasApparel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //to get the numbers of product in carts get the logged in user ID
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claim = userIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                //access the DB and get the count of products from the logged in user associated shopping cart
                var count = _unitOfWork.ShoppingCarts.GetAll().Where(x => x.NoheasApparelUserID == claim.Value).ToList().Count();

                //store the count in session
                HttpContext.Session.SetInt32(StaticDetail.SessionShopCart, count);
            }

            return View();
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }

   
    }
}