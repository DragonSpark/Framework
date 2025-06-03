using System;
using DragonSpark.Compose;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Syncfusion;

sealed class ConfigureApplication : IMauiInitializeService
{
    readonly RegisterLicense _license;

    public ConfigureApplication(RegisterLicense license) => _license = license;

    public void Initialize(IServiceProvider services)
    {
        _license.Execute();
    }
}