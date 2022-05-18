using DragonSpark.Runtime;
using System;

namespace DragonSpark.Application.Diagnostics.Time;

sealed class Outside : IWindow
{
	readonly ITime    _time;
	readonly TimeSpan _window;

	public Outside(TimeSpan window) : this(DragonSpark.Runtime.Time.Default, window) {}

	public Outside(ITime time, TimeSpan window)
	{
		_time   = time;
		_window = window;
	}

	public bool Get(DateTimeOffset parameter) => parameter >= _time.Get() + _window;
}