using AbpDemo.Books;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpDemo.Products
{
    public class CreateUpdateProductDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = string.Empty;

       
        [Required]
        public decimal Price { get; set; }
    }
}
