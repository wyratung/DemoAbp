using AutoMapper;
using AbpDemo.Books;
using AbpDemo.Products;

namespace AbpDemo;

public class AbpDemoApplicationAutoMapperProfile : Profile
{
    public AbpDemoApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
