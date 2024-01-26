namespace NoheasApparel.Utility
{
    public static class StaticDetail
    {
        //public static decimal GetPrice(decimal qty, decimal price)
        //{ 
            
        //}

       //added customize roles
       public const string Role_User_Customer = "Customer";
        public const string Role_Employee = "Employee";
        public const string Role_Admin = "Admin";
        public const string Role_Company = "Company";

        public const string SessionShopCart = "Shoping Cart Session";

        public const string PendingStatus = "Pending";
        public const string ApprovedStatus = "Approved";
        public const string ProcessStatus = "Processing";
        public const string ShippedStatus = "Shipped";
        public const string CancelledStatus = "Cancelled";
        public const string RefundStatus = "Refunded";

        public const string PendingPaymentStatus = "Pending";
        public const string ApprovedPaymentStatus = "Approved";
        public const string DelayedPaymentStatus = "Delayed Payment";
        public const string RejectedPaymentStatus = "Rejected";

    }
}