using System;
using DragonSpark.Application.Mobile.Maui.Run;
using DragonSpark.Composition;
using DragonSpark.Model.Selection;
using Microsoft.Maui.Hosting;
using Sentry;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class InitializationAwareRunApplication : IRunApplication
{
    readonly ISelect<MauiAppBuilder, MauiApp> _previous;

    public InitializationAwareRunApplication(ISelect<MauiAppBuilder, MauiApp> previous) => _previous = previous;

    public MauiApp Get(MauiAppBuilder parameter)
    {
        try
        {
            return _previous.Get(parameter);
        }
        catch (Exception e)
        {
            var address = parameter.Services.Configuration().Section<InitializationLoggingSettings>() 
                              is { Enabled: true, Address: not null and not "" } s ? s.Address : null;
            if (address is not null)
            {
                using var _ = SentrySdk.Init(address);
                SentrySdk.CaptureException(e);
            }
            throw;
        }
    }
}