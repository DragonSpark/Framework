using DragonSpark.Model.Results;
using Serilog;
using Serilog.Events;

namespace DragonSpark.Diagnostics;

sealed class Logger : ILogger
{
    readonly IResult<ILogger?> _store;
    readonly ILogger           _default;

    public Logger(IResult<ILogger?> store) : this(store, Log.Logger) {}

    public Logger(IResult<ILogger?> store, ILogger @default)
    {
        _store   = store;
        _default = @default;
    }

    public void Write(LogEvent logEvent)
    {
        var logger = _store.Get() ?? _default;
        logger.Write(logEvent);
    }
}