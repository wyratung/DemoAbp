using Microsoft.AspNetCore.Builder;
using AbpDemo;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("AbpDemo.Web.csproj"); 
await builder.RunAbpModuleAsync<AbpDemoWebTestModule>(applicationName: "AbpDemo.Web");

public partial class Program
{
}
