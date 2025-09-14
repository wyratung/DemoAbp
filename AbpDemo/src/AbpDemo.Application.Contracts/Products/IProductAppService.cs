using AbpDemo.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpDemo.Products
{
    public interface IProductAppService :
    ICrudAppService< //Defines CRUD methods
        ProductDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateProductDto> //Used to create/update a book
    {
    }
}
