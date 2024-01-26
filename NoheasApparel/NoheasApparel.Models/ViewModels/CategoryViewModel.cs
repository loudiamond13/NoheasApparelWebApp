using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoheasApparel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NoheasApparel.Models.ViewModels
{
    public class CategoryViewModel
    {
        public List<Product>? Products { get; set; }
        public Category? Category { get; set; }
    }
}
