using System.ComponentModel.DataAnnotations;

namespace NoheasApparel.Models
{
    public class Gender
    {
        [Key]
        public int GenderID { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string GenderName { get; set; } = string.Empty;
    }
}
