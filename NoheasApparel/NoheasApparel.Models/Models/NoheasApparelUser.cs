using Microsoft.AspNetCore.Identity;
using NoheasApparel.Models.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoheasApparel.Models
{
    public class NoheasApparelUser : IdentityUser
    {


        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string StreetAddress { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string State { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
     
        public int? CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public Company? Company { get; set;}

        [NotMapped]
        public string Role { get; set; } = string.Empty;
    }
}
