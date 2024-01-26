using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoheasApparel.Models.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? StreetAddress { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? PostalCode { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public bool IsAuthorizedCompany { get; set; } 
    }
}
