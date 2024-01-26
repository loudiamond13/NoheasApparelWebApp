using NoheasApparel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NoheasApparel.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Brands { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Genders { get; set; }





        public int ProductID { get; set; } 
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Color { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
