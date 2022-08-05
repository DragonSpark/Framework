using DragonSpark.Runtime;
using System;

namespace DragonSpark.Application.Diagnostics.Time;

sealed class FromNow : IWindow
{
	readonly ITime    _time;
	readonly TimeSpan _window;

	public FromNow(TimeSpan window) : this(DragonSpark.Runtime.Time.Default, window) {}

	public FromNow(ITime time, TimeSpan window)
	{
		_time   = time;
		_window = window;
	}

	public bool Get(DateTimeOffset parameter) => parameter <= _time.Get() + _window;
}

sealed class FromThen : IWindow
{
	readonly ITime    _time;
	readonly TimeSpan _window;

	public FromThen(TimeSpan window) : this(DragonSpark.Runtime.Time.Default, window) {}

	public FromThen(ITime time, TimeSpan window)
	{
		_time   = time;
		_window = window;
	}

	public bool Get(DateTimeOffset parameter) => _time.Get() >= parameter - _window;
}