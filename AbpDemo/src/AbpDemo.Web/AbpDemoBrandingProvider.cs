using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using AbpDemo.Localization;

namespace AbpDemo.Web;

[Dependency(ReplaceServices = true)]
public class AbpDemoBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AbpDemoResource> _localizer;

    public AbpDemoBrandingProvider(IStringLocalizer<AbpDemoResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
