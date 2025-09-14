using AbpDemo.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpDemo.Products
{
    public class Product : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }        

        public decimal Price { get; set; }
    }
}
