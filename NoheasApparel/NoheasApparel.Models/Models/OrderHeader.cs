using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoheasApparel.Models.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderID { get; set; }

        public string NoheasApparelUserID { get; set; } = string.Empty;
        [ForeignKey("NoheasApparelUserID")]
        public NoheasApparelUser NoheasApparelUser { get; set; }


        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "Shipping Date")]
        public DateTime ShippingDate { get; set; }

        [Display(Name = "Order Total")]
        public decimal OrderTotal { get; set; }

        public decimal Tax { get; set; }

        public decimal SubTotal { get; set; }

        [Display(Name = "Tracking Number")]
        public string TrackingNumber { get; set; }  = string.Empty;

        public string Carrier { get; set; } = string.Empty;
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set;} = string.Empty;

        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; } = string.Empty;

        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Payment Due Date")]
        public DateTime PaymentDueDate { get; set;} 



        [Display(Name = "Transaction ID")]
        public string TransactionID { get; set; } = string.Empty;


        
        [StringLength(80)]
        [Required]
        public string Name { get; set; }

       
        [StringLength(50)]
        [Display(Name = "Street Address")]
        [Required]
        public string StreetAddress { get; set; } 

        
        [StringLength(50)]
        [Required]
        public string City { get; set; } 

        
        [StringLength(50)]
        [Required]
        public string State { get; set; } 

       
        [StringLength(20)]
        [Display(Name = "Postal Code")]
        [Required]
        public string PostalCode { get; set; } 

        
        [StringLength(13)]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$",
        ErrorMessage = "Phone Number Must Be In 000-000-0000 Format.")]
        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }
    }
}
