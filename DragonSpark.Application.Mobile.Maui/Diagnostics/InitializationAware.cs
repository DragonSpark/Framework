using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Runtime.Initialization;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;
using Sentry;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

public class InitializationAware<TIn, TOut> : ISelect<TIn, TOut>
{
    readonly ISelect<TIn, TOut>        _previous;
    readonly Func<TIn, IConfiguration> _configuration;

    protected InitializationAware(ISelect<TIn, TOut> previous, Func<TIn, IConfiguration> configuration)
    {
        _previous      = previous;
        _configuration = configuration;
    }

    public TOut Get(TIn parameter)
    {
        try
        {
            return _previous.Get(parameter);
        }
        catch (Exception e)
        {
            var address = _configuration(parameter).Section<InitializationLoggingSettings>() is
                              { Enabled: true, Address: not null and not "" } s
                              ? s.Address
                              : null;
            if (address is not null)
            {
                using var _ = SentrySdk.Init(address);
                SentrySdk.CaptureException(e);
            }

            throw;
        }
    }
}

public class InitializationAware : IOperation
{
    readonly IConfiguration _configuration;
    readonly IOperation     _previous;

    protected InitializationAware(IConfiguration configuration, IOperation previous)
    {
        _configuration = configuration;
        _previous      = previous;
    }

    public async ValueTask Get()
    {
        try
        {
            await _previous.On();
        }
        catch (Exception e)
        {
            var address = _configuration.Section<InitializationLoggingSettings>() is
                              { Enabled: true, Address: not null and not "" } s
                              ? s.Address
                              : null;
            if (address is not null)
            {
                using var _ = SentrySdk.Init(address);
                SentrySdk.CaptureException(e);
            }

            throw;
        }
    }
}

// TODO

public sealed class InitializeApplication : InitializationAware
{
    public InitializeApplication(IConfiguration configuration) : base(configuration, PerformInitialization.Default) {}
}