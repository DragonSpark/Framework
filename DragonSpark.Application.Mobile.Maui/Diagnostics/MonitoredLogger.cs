using System.Buffers;
using System.Collections.Generic;
using NetFabric.Hyperlinq;
using Serilog;
using Serilog.Events;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class MonitoredLogger : IMonitoredLogger
{
    readonly List<LogEvent> _history;

    public MonitoredLogger() : this([]) {}

    public MonitoredLogger(List<LogEvent> history) => _history = history;

    public void Write(LogEvent logEvent)
    {
        _history.Add(logEvent);
    }

    public void Execute(ILogger parameter)
    {
        using var lease = _history.AsValueEnumerable().ToArray(ArrayPool<LogEvent>.Shared);
        if (lease.Length > 0)
        {
            _history.Clear();
            parameter.Information("Initializing log with {Count} entries", lease.Length);
            foreach (var @event in lease)
            {
                parameter.Write(@event);
            }
        }
    }
}