using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace AbpDemo.Pages;

[Collection(AbpDemoTestConsts.CollectionDefinitionName)]
public class Index_Tests : AbpDemoWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
