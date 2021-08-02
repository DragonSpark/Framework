using DragonSpark.Runtime;
using System;

namespace DragonSpark.Application.Diagnostics.Time
{
	sealed class Ago : IWindow
	{
		readonly ITime    _time;
		readonly TimeSpan _window;

		public Ago(TimeSpan window) : this(DragonSpark.Runtime.Time.Default, window) {}

		public Ago(ITime time, TimeSpan window)
		{
			_time   = time;
			_window = window;
		}

		public bool Get(DateTimeOffset parameter) => parameter >= _time.Get() - _window;
	}
}