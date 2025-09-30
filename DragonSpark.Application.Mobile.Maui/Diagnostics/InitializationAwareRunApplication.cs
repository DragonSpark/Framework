using DragonSpark.Application.Mobile.Maui.Run;
using DragonSpark.Model.Selection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class InitializationAwareRunApplication : InitializationAware<MauiAppBuilder, MauiApp>, IRunApplication
{
    public InitializationAwareRunApplication(ISelect<MauiAppBuilder, MauiApp> previous)
        : base(previous, x => x.Configuration) {}
}