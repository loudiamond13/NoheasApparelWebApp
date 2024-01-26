using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoheasApparel.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Please enter the product name.")]
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Product Description")]
        [Required(ErrorMessage = "Please enter the product description.")]
        public string ProductDescription { get; set; } = string.Empty;


        [Display(Name = "Product Price")]
        [Column(TypeName = "decimal(8,2)")]
        [Required(ErrorMessage = "Please enter the product price.")]
        [Range(1, 5000, ErrorMessage = "Product price must be > $0.00 and =< 5000")]
        public decimal ProductPrice { get; set; }

        [Display(Name = "Product Color")]
        [Required(ErrorMessage = "Please enter the product color.")]
        public string ProductColor { get; set; } = string.Empty;



        



        [Display(Name = "Gender")]
        public int GenderID { get; set; }



        [Display(Name = "Brand")]
        public int BrandID { get; set; }



        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        //navigation properties
        //[ForeignKey("BrandID")]
        [ValidateNever]
        public Brand Brand { get; set; }
        [ForeignKey("GenderID")]
        [ValidateNever]
        public Gender? Gender { get; set; }

        [ForeignKey("CategoryID")]
        
        [ValidateNever]
        public Category? Category { get; set; }




        [Display(Name = "Product Image")]
        [ValidateNever]
        public string? ProductImageURL { get; set; } = string.Empty;
    }
}
