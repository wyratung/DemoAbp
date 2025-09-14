using AbpDemo.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace AbpDemo.Web.Pages;

public abstract class AbpDemoPageModel : AbpPageModel
{
    protected AbpDemoPageModel()
    {
        LocalizationResourceType = typeof(AbpDemoResource);
    }
}
