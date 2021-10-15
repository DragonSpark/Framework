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