using System;
using DragonSpark.Model.Commands;
using Syncfusion.Licensing;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Syncfusion;

sealed class RegisterLicense : FixedParameterCommand<string>
{
    public RegisterLicense(SyncfusionConfiguration configuration)
        : this(configuration.License, SyncfusionLicenseProvider.RegisterLicense) {}

    public RegisterLicense(string configuration, Action<string> command) : base(command, configuration) {}
}