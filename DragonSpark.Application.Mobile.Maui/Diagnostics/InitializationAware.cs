using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

public class InitializationAware<TIn, TOut> : ISelect<TIn, TOut>
{
    readonly ISelect<TIn, TOut> _previous;
    readonly IReport<TIn>       _report;

    protected InitializationAware(ISelect<TIn, TOut> previous, Func<TIn, IConfiguration> configuration)
        : this(previous, new Report<TIn>(configuration)) {}

    protected InitializationAware(ISelect<TIn, TOut> previous, IReport<TIn> report)
    {
        _previous = previous;
        _report   = report;
    }

    public TOut Get(TIn parameter)
    {
        try
        {
            return _previous.Get(parameter);
        }
        catch (Exception e)
        {
            _report.Execute(new(parameter, e));
            throw;
        }
    }
}

public class InitializationAware : IStopAware
{
    readonly IStopAware _previous;
    readonly IReport    _send;

    protected InitializationAware(IStopAware previous, IReport send)
    {
        _previous = previous;
        _send     = send;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        try
        {
            await _previous.On(parameter);
        }
        catch (Exception e)
        {
            _send.Execute(e);
            throw;
        }
    }
}