using AbpDemo.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace AbpDemo.Products
{
    public class ProductDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }    

        public decimal Price { get; set; }
    }
}
