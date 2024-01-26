using System.ComponentModel.DataAnnotations;

namespace NoheasApparel.Models
{
    public class Brand
    {
        [Key]
        public int BrandID { get; set; }

      
        [Required]
        [MaxLength(40)]
        [Display(Name = "Product Brand")]
        public string BrandName { get; set; } = string.Empty;
    }
}
