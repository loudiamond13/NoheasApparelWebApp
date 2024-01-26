using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoheasApparel.Models.Models
{
    public class ShoppingCart
    {
       

        [Key]
        public int CartID { get; set; }
        public string NoheasApparelUserID { get; set; } = string.Empty;

        [ValidateNever]
        [ForeignKey("NoheasApparelUserID")]
        public NoheasApparelUser? NoheasApparelUser { get; set; }


        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product? Product { get; set; }

        [Range(1,1000)]
        public int Count { get; set; }

        public ShoppingCart()
        {
            Count = 1;
        }

        [NotMapped]
        public decimal Price { get; set; }

    }
}
