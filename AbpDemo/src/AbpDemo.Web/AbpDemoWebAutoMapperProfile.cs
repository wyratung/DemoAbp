using AutoMapper;
using AbpDemo.Books;

namespace AbpDemo.Web;

public class AbpDemoWebAutoMapperProfile : Profile
{
    public AbpDemoWebAutoMapperProfile()
    {
        CreateMap<BookDto, CreateUpdateBookDto>();
        
        //Define your object mappings here, for the Web project
    }
}
