using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Timers;

namespace DragonSpark.Application.Runtime;

public class Throttling<T> : IThrottling<T>
{
	readonly ITable<T, Timer> _timers;
	readonly TimeSpan         _duration;

	public Throttling(ITable<T, Timer> timers) : this(timers, TimeSpan.FromMilliseconds(750)) {}

	public Throttling(ITable<T, Timer> timers, TimeSpan duration)
	{
		_timers   = timers;
		_duration = duration;
	}

	public void Execute(Throttle<T> parameter)
	{
		var (input, action) = parameter;
		var result = _timers.Get(input).Account();
		if (result == null)
		{
			result = new Timer { Enabled = false, AutoReset = false, Interval = _duration.TotalMilliseconds };
			// Who am I to argue: https://stackoverflow.com/questions/38917818/pass-async-callback-to-timer-constructor#comment91001639_38918443
			result.Elapsed += async (_, _) =>
			                  {
				                  try
				                  {
					                  await action(input);
				                  }
				                  finally
				                  {
					                  _timers.Remove(input);
				                  }
			                  };
			_timers.Assign(input, result);
		}

		result.Stop();
		result.Start();
	}
}

public class Throttling : ICommand
{
	readonly Timer _timer;

	public Throttling(Operate operate) : this(operate, TimeSpan.FromMilliseconds(750)) {}

	public Throttling(Operate operate, TimeSpan window)
		: this(operate, new Timer { Enabled = false, AutoReset = false, Interval = window.TotalMilliseconds }) {}

	public Throttling(Operate operate, Timer timer)
	{
		_timer        =  timer;
		// Who am I to argue: https://stackoverflow.com/questions/38917818/pass-async-callback-to-timer-constructor#comment91001639_38918443
		timer.Elapsed += async (_, _) => await operate();
	}

	public void Execute(None parameter)
	{
		_timer.Stop();
		_timer.Start();
	}
}