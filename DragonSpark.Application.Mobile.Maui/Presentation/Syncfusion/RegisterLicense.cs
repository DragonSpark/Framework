using DragonSpark.Model.Commands;
using Syncfusion.Licensing;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Syncfusion;

sealed class RegisterLicense : FixedParameterCommand<string>
{
    public RegisterLicense(SyncfusionConfiguration configuration)
        : base(SyncfusionLicenseProvider.RegisterLicense, configuration.License) {}
}