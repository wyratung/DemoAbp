using AbpDemo.Books;
using AbpDemo.Products;
using Xunit;

namespace AbpDemo.EntityFrameworkCore.Applications.Books;

[Collection(AbpDemoTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : DemoAppService_Tests<AbpDemoEntityFrameworkCoreTestModule>
{

}
