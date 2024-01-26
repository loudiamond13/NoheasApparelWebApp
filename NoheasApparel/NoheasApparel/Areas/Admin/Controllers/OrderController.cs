using NoheasApparel.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoheasApparel.Utility;
using NoheasApparel.Models.Models;
using System.Security.Claims;
using NoheasApparel.Models.ViewModels;
using Stripe;

namespace NoheasApparel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

      
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            
            OrderDetailsVM orderDetailsVM = new OrderDetailsVM()
            {
                OrderDetails = _unitOfWork.OrderDetails.GetAll(includeProperties: "Product").Where(x => x.OrderHeaderID == id).ToList(),
                OrderHeader = _unitOfWork.OrderHeaders.Find(x => x.OrderHeaderID == id, includeProperties: "NoheasApparelUser")
            };

            return View(orderDetailsVM);
        }


        [Authorize(Roles = StaticDetail.Role_Employee + "," + StaticDetail.Role_Admin)]
        public IActionResult Process(int id)
        {
            //get the order
            OrderHeader orderHeader = _unitOfWork.OrderHeaders.Find(x => x.OrderHeaderID == id);

            //set the status of the order to processing 
            orderHeader.OrderStatus = StaticDetail.ProcessStatus;
            //save DB
            _unitOfWork.Save();
            TempData["success"] = "Order Processed successfully.";
            return RedirectToAction("Index");
        }


        //ship order action
        [HttpPost]
        [Authorize(Roles = StaticDetail.Role_Employee + "," + StaticDetail.Role_Admin)]
        public IActionResult Ship(OrderDetailsVM orderDetailsVM)
        {
            
            //get the order
            OrderHeader orderHeader = _unitOfWork.OrderHeaders.Find(x => x.OrderHeaderID == orderDetailsVM.OrderHeader.OrderHeaderID);

            //populate the orderheader
            orderHeader.TrackingNumber = orderDetailsVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = orderDetailsVM.OrderHeader.Carrier;
            orderHeader.ShippingDate = DateTime.Now;
            orderHeader.OrderStatus = StaticDetail.ShippedStatus;

            _unitOfWork.OrderHeaders.Update(orderHeader);
            _unitOfWork.Save();
            TempData["success"] = "Order Shipped successfully.";
            return RedirectToAction("Index");
        }


        //cancel order action
        [Authorize(Roles = StaticDetail.Role_Employee + "," + StaticDetail.Role_Admin)]
        public IActionResult CancelOrder(int id)
        {
            //get the order
            OrderHeader orderHeader = _unitOfWork.OrderHeaders.Find(x => x.OrderHeaderID == id);

            //do a refund process if payment was approved
            if (orderHeader.PaymentStatus == StaticDetail.ApprovedStatus)
            {
                //process the refund
                var options = new RefundCreateOptions
                {
                    Amount = Convert.ToInt32(orderHeader.OrderTotal * 100),
                    Reason = RefundReasons.RequestedByCustomer,
                    Charge = orderHeader.TransactionID
                };

                var service = new RefundService();
                Refund refund = service.Create(options);
                //if it was paid successfully, set the status to refunded
                orderHeader.OrderStatus = StaticDetail.RefundStatus;
                orderHeader.PaymentStatus = StaticDetail.RefundStatus;
            }
            //else if payment wasnt approved, set the statuses
            else 
            {
                //set the order status and payment status
                orderHeader.OrderStatus = StaticDetail.CancelledStatus;
                orderHeader.PaymentStatus= StaticDetail.CancelledStatus;
            }

            TempData["success"] = "Order cancelled successfully.";
            //save DB
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }




        #region api calls
        public IActionResult Order(string status) 
        {
            //get the user identity
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claim = userIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<OrderHeader> orderHeaders = new List<OrderHeader>();

            //check if logged in user is employee or admin
            //if logged in user is employee || admin, show all the orders
            if (User.IsInRole(StaticDetail.Role_Employee) || User.IsInRole(StaticDetail.Role_Admin))
            {
                //get all the orders from the DB
                orderHeaders = _unitOfWork.OrderHeaders.GetAll(includeProperties: "NoheasApparelUser").ToList();
            }
            //else if the user is customer, show just their own order
            else 
            {
                orderHeaders = _unitOfWork.OrderHeaders.GetAll(includeProperties: "NoheasApparelUser")
                                .Where(x => x.NoheasApparelUserID == claim.Value).ToList();
            }


            //organize orders based on their statuses
            switch (status)
            {
                case "Completed":   //if order is shipped, put it in completed
                    orderHeaders = orderHeaders.Where(x => x.OrderStatus == StaticDetail.ShippedStatus).ToList();
                    break;
                case "Rejected":    //if order is refunded || cancelled || rejected payment, put it in rejected
                    orderHeaders = orderHeaders.Where(x => x.OrderStatus == StaticDetail.RejectedPaymentStatus
                                                    || x.OrderStatus == StaticDetail.RefundStatus
                                                    || x.OrderStatus == StaticDetail.CancelledStatus).ToList();
                    break;
                case "Inprogress":  // if order is in progress || pending, put it in inprogress
                    orderHeaders = orderHeaders.Where(x => x.OrderStatus == StaticDetail.ProcessStatus 
                                                    || x.OrderStatus == StaticDetail.PendingStatus 
                                                    || x.OrderStatus == StaticDetail.ApprovedStatus).ToList();
                    break;
                case "Pending": // if payment is in delayed, put it in pending
                    orderHeaders = orderHeaders.Where(x => x.PaymentStatus == StaticDetail.DelayedPaymentStatus).ToList();
                    break;
                default:
                    break;
            }

            return Json(new { data = orderHeaders });
        
        }
        #endregion

    }
}
