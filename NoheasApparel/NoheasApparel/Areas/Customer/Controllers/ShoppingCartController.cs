using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoheasApparel.DataAccess.Repository.Interfaces;

using NoheasApparel.Models;
using NoheasApparel.Models.Models;
using NoheasApparel.Models.ViewModels;
using NoheasApparel.Utility;
using Stripe;
using System.Security.Claims;

namespace NoheasApparel.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        

        public ShoppingCartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;   
        }

        //index action of the shopping cart
        public IActionResult Index()
        {
             var userIdentity = (ClaimsIdentity)User.Identity;
            var claim = userIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // fill the shopping cart view model
            ShoppingCartVM shoppingCartVM = new ShoppingCartVM() 
            {
                OrderHeader = new OrderHeader(),
                //get the shopping cart from DB where the user ID matched/associated
                ShoppingCarts = _unitOfWork.ShoppingCarts.GetAll(includeProperties: "Product")
                                            .Where(x => x.NoheasApparelUserID == claim.Value)
                
            };
            //clear the ordertotal
            shoppingCartVM.OrderHeader.OrderTotal = 0;
            
            shoppingCartVM.OrderHeader.NoheasApparelUser = _unitOfWork.NoheasApparelUsers
                                                            .Find(x => x.Id == claim.Value);

            foreach (var cart in shoppingCartVM.ShoppingCarts)
            {
              
               
                //shoppingCartVM.Products = _unitOfWork.Products.GetAll().Where(x => x.ProductID == cart.ProductID).ToList();
               shoppingCartVM.OrderHeader.OrderTotal += (cart.Product.ProductPrice * cart.Count);

            }

      

            return View(shoppingCartVM);
        }

        //the plus button in shopping cart action
        public IActionResult Plus(int cartID)
        {
            //get the shopping cart from DB
            ShoppingCart shoppingCart = _unitOfWork.ShoppingCarts
                                        .Find(x => x.CartID == cartID, includeProperties: "Product");
            
            //add count to the Shopping cart
            shoppingCart.Count += 1;

            //update the db
            _unitOfWork.ShoppingCarts.Update(shoppingCart);
            _unitOfWork.Save();
            return RedirectToAction("Index");  
        }

        public IActionResult Minus(int cartID)
        {
            //get the cart from DB
            ShoppingCart shoppingCart = _unitOfWork.ShoppingCarts
                                        .Find(x => x.CartID == cartID, includeProperties: "Product");

            //if there is only one item of the product, remove it from cart/DB
            if (shoppingCart.Count == 1)
            {
                //update the session count
                int c = _unitOfWork.ShoppingCarts.GetAll().Where(x => x.NoheasApparelUserID == shoppingCart.NoheasApparelUserID).ToList().Count();

                
                _unitOfWork.ShoppingCarts.Delete(shoppingCart);
                _unitOfWork.Save();

                //update the count of the cart in sesstion 
                HttpContext.Session.SetInt32(StaticDetail.SessionShopCart, c - 1);

                TempData["success"] = "Product Successfully Removed From Shopping Cart.";
            }
            //if there is > 1 item in for the product that is passed in, minus 1/take 1 
            else
            {
                shoppingCart.Count -= 1;
                _unitOfWork.ShoppingCarts.Update(shoppingCart);
                _unitOfWork.Save();

            }
            
          
            return RedirectToAction("Index");
        }


        public IActionResult Remove(int cartID) 
        {
            //get the shopping cart from DB
            ShoppingCart shoppingCart = _unitOfWork.ShoppingCarts
                                        .Find(x => x.CartID == cartID, includeProperties: "Product");

            //update the session count
            int c = _unitOfWork.ShoppingCarts.GetAll().Where(x => x.NoheasApparelUserID == shoppingCart.NoheasApparelUserID).ToList().Count();

            //delete the cart in DB
            _unitOfWork.ShoppingCarts.Delete(shoppingCart);
            //save DB
            _unitOfWork.Save();

            //minus 1 for the session int for the cart
            HttpContext.Session.SetInt32(StaticDetail.SessionShopCart, c - 1);

            TempData["success"] = "Product Successfully Removed From Shopping Cart.";


            //redirect to index
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult CheckOut()
        {
            var identityUser = (ClaimsIdentity)User.Identity;
            var claim = identityUser.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM() 
            {
               OrderHeader  = new OrderHeader(),
               ShoppingCarts = _unitOfWork.ShoppingCarts.GetAll(includeProperties: "Product").Where(x => x.NoheasApparelUserID == claim.Value)
               
            };

            //fill in the information from the user information store in DB
            shoppingCartVM.OrderHeader.NoheasApparelUser = _unitOfWork.NoheasApparelUsers.Find(x => x.Id == claim.Value);
            shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.NoheasApparelUser.Name;
            shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.NoheasApparelUser.StreetAddress;
            shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.NoheasApparelUser.City;
            shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.NoheasApparelUser.PostalCode;
            shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.NoheasApparelUser.State;
            shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.NoheasApparelUser.PhoneNumber;

            //gets the total price of all products that are in the cart
            foreach (var cart in shoppingCartVM.ShoppingCarts)
            {
                //shoppingCartVM.Products = _unitOfWork.Products.GetAll().Where(x => x.ProductID == cart.ProductID).ToList();
                shoppingCartVM.OrderHeader.SubTotal += (cart.Product.ProductPrice * cart.Count);
            }
            //calculate the tax 
            shoppingCartVM.OrderHeader.Tax = shoppingCartVM.OrderHeader.SubTotal * 10/100;
            //calculate the order total
            shoppingCartVM.OrderHeader.OrderTotal = shoppingCartVM.OrderHeader.SubTotal + shoppingCartVM.OrderHeader.Tax;

            return View("CheckOut", shoppingCartVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CheckOut")]
        public IActionResult CheckOut(ShoppingCartVM shoppingCartVM, string stripeTOKEN)
        { 
            //get the user id
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claim = userIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            //populate the orderHeader, make payments pending as of now
            shoppingCartVM.OrderHeader.NoheasApparelUser = _unitOfWork.NoheasApparelUsers.Find(x => x.Id == claim.Value);
            shoppingCartVM.OrderHeader.PaymentStatus = StaticDetail.PendingPaymentStatus;
            shoppingCartVM.OrderHeader.NoheasApparelUserID = claim.Value;
            shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            //add the populated order header into the DB
            _unitOfWork.OrderHeaders.Add(shoppingCartVM.OrderHeader);
            _unitOfWork.Save();
            
            
            //get the users cart
            shoppingCartVM.ShoppingCarts = _unitOfWork.ShoppingCarts.GetAll(includeProperties: "Product")
                                                    .Where(x => x.NoheasApparelUserID == claim.Value);

            
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            //fill the order detail for each product/item in cart so it can be added into the db
            foreach (var item in shoppingCartVM.ShoppingCarts)
            { 
                OrderDetail orderDetail = new OrderDetail() 
                {
                    ProductID = item.ProductID,
                    OrderHeaderID = shoppingCartVM.OrderHeader.OrderHeaderID,
                    Price = item.Product.ProductPrice,
                    Count = item.Count
                };

               

                //add the populated order details into the DB
                _unitOfWork.OrderDetails.Add(orderDetail);
               
            }
            //update the sub Total,tax,orderTotal of the orderheader in DB
            shoppingCartVM.OrderHeader.SubTotal = shoppingCartVM.OrderHeader.SubTotal;
            shoppingCartVM.OrderHeader.Tax = shoppingCartVM.OrderHeader.Tax;
            shoppingCartVM.OrderHeader.OrderTotal = shoppingCartVM.OrderHeader.OrderTotal;

            //delete the whole shopping cart of the logged in user and save DB
            _unitOfWork.ShoppingCarts.DeleteRange(shoppingCartVM.ShoppingCarts);
            //save the DB
            

            //clear the number for the cart
            HttpContext.Session.SetInt32(StaticDetail.SessionShopCart, 0);

            //if token is not null, process the payment
            if (stripeTOKEN != null)
            {
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(shoppingCartVM.OrderHeader.OrderTotal * 100),
                    Currency = "usd",
                    Description = "Order ID: " + shoppingCartVM.OrderHeader.OrderHeaderID,
                    Source = stripeTOKEN
                    

                };

                var service =   new ChargeService();
                Charge charge = service.Create(options); // do the transaction

                //populate the billing details with the current user information
                charge.BillingDetails.Phone = shoppingCartVM.OrderHeader.PhoneNumber;
                charge.BillingDetails.Email = shoppingCartVM.OrderHeader.NoheasApparelUser.Email;
                charge.BillingDetails.Address.City = shoppingCartVM.OrderHeader.City;
                charge.BillingDetails.Address.PostalCode = shoppingCartVM.OrderHeader.PostalCode;
                charge.BillingDetails.Address.Line1 = shoppingCartVM.OrderHeader.StreetAddress;
                charge.BillingDetails.Address.State = shoppingCartVM.OrderHeader.State;

                //if transaction ID is null, payment was rejected
                if (charge.BalanceTransactionId == null)
                {
                    //set the rejected status
                    shoppingCartVM.OrderHeader.PaymentStatus = StaticDetail.RejectedPaymentStatus;
                }
                //else if transaction id is not null, store the transaction ID
                else 
                {
                    shoppingCartVM.OrderHeader.TransactionID = charge.Id;
                }

                //if the status is succeeded, update the orderheader information
                if ("succeeded" == charge.Status.ToLower())
                {
                    shoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
                    shoppingCartVM.OrderHeader.PaymentStatus = StaticDetail.ApprovedPaymentStatus;
                    shoppingCartVM.OrderHeader.OrderStatus = StaticDetail.ApprovedStatus;
                }
            }

            //save the DB
            _unitOfWork.Save();
            //redirect to the confirmation page with the orderheaderID( order # )
            return RedirectToAction("OrderConfirm","ShoppingCart", new {id = shoppingCartVM.OrderHeader.OrderHeaderID });
        }


        public IActionResult OrderConfirm(int id)
        { 
            return View(id);
        }
    }
}
