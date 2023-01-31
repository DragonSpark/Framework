using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Application.Runtime;

public class Throttling<T> : IThrottling<T>
{
	readonly ITable<DelegateKey<T>, ThrottleContext>  _timers;
	readonly ISelect<DelegateKey<T>, ThrottleContext> _create;

	public Throttling() : this(TimeSpan.FromMilliseconds(750)) {}

	public Throttling(TimeSpan duration) : this(Delegates<T>.Default, duration) {}

	public Throttling(ITable<DelegateKey<T>, ThrottleContext> timers, TimeSpan interval)
		: this(timers, new CreateThrottleContext<T>(timers, interval.TotalMilliseconds)) {}

	public Throttling(ITable<DelegateKey<T>, ThrottleContext> timers, ISelect<DelegateKey<T>, ThrottleContext> create)
	{
		_timers = timers;
		_create = create;
	}

	public void Execute(Throttle<T> parameter)
	{
		var (p, operate, source) = parameter;
		var key     = new DelegateKey<T>(operate, p);
		var context = _timers.Get(key).Account() ?? _create.Get(key);
		var (subject, task) = context;
		task.Add(source);
		subject.Stop();
		subject.Start();
	}
}

sealed class Delegates<T> : ConcurrentTable<DelegateKey<T>, ThrottleContext>
{
	public static Delegates<T> Default { get; } = new();

	Delegates() {}
}

public readonly record struct DelegateKey<T>(Operate<T> Reference, T Parameter);

/*public class Throttling : ICommand
{
	readonly Timer _timer;

	//public Throttling(Operate operate) : this(operate, TimeSpan.FromMilliseconds(750)) {}

	public Throttling(Operate operate, TimeSpan window)
		: this(operate, new Timer { Enabled = false, AutoReset = false, Interval = window.TotalMilliseconds }) {}

	public Throttling(Operate operate, Timer timer)
	{
		_timer = timer;
		// Who am I to argue: https://stackoverflow.com/questions/38917818/pass-async-callback-to-timer-constructor#comment91001639_38918443
		timer.Elapsed += (_, _) => operate();
	}

	public void Execute(None parameter)
	{
		_timer.Stop();
		_timer.Start();
	}
}*/