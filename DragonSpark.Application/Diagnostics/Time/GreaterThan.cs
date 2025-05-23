using System;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Diagnostics.Time;

sealed class GreaterThan : IWindow
{
    readonly ITime    _time;
    readonly TimeSpan _window;

    public GreaterThan(TimeSpan window) : this(DragonSpark.Runtime.Time.Default, window) {}

    public GreaterThan(ITime time, TimeSpan window)
    {
        _time   = time;
        _window = window;
    }

    public bool Get(DateTimeOffset parameter) => parameter - _time.Get() >= _window;
}