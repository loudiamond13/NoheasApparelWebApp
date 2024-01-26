using System.ComponentModel.DataAnnotations;

namespace NoheasApparel.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

     
        [Required]
        [MaxLength(50)]
        [Display(Name = "Product Category")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
