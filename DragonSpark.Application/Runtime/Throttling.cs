using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace DragonSpark.Application.Runtime;

public class Throttling<T> : IThrottling<T>
{
	readonly ITable<Delegate, ThrottleContext>     _timers;
	readonly ISelect<Throttle<T>, ThrottleContext> _create;

	public Throttling() : this(TimeSpan.FromMilliseconds(750)) {}

	public Throttling(TimeSpan duration) : this(new Table<Delegate, ThrottleContext>(), duration) {}

	public Throttling(ITable<Delegate, ThrottleContext> timers, TimeSpan interval)
		: this(timers, new CreateThrottleContext<T>(timers, interval.TotalMilliseconds)) {}

	public Throttling(ITable<Delegate, ThrottleContext> timers, ISelect<Throttle<T>, ThrottleContext> create)
	{
		_timers = timers;
		_create = create;
	}

	public void Execute(Throttle<T> parameter)
	{
		var (_, key, source) = parameter;
		var context = _timers.Get(key).Account() ?? _create.Get(parameter);
		var (subject, task) = context;
		task.Add(source);
		subject.Stop();
		subject.Start();
	}
}

// TODO

sealed class CreateThrottleContext<T> : ISelect<Throttle<T>, ThrottleContext>
{
	readonly ITable<Delegate, ThrottleContext> _timers;
	readonly double                            _interval;

	public CreateThrottleContext(ITable<Delegate, ThrottleContext> timers, double interval)
	{
		_timers   = timers;
		_interval = interval;
	}

	public ThrottleContext Get(Throttle<T> parameter)
	{
		var (input, key, _) = parameter;
		var sources = new HashSet<TaskCompletionSource>();
		var timer   = new Timer { Enabled = false, AutoReset = false, Interval = _interval };
		// Who am I to argue: https://stackoverflow.com/questions/38917818/pass-async-callback-to-timer-constructor#comment91001639_38918443
		timer.Elapsed += async (_, _) =>
		                 {
			                 using var items = sources.AsValueEnumerable()
			                                          .ToArray(ArrayPool<TaskCompletionSource>.Shared);
			                 try
			                 {
				                 await key(input);
				                 foreach (var s in items)
				                 {
					                 s.TrySetResult();
				                 }
			                 }
			                 catch (Exception e)
			                 {
				                 foreach (var s in items)
				                 {
					                 s.TrySetException(e);
				                 }
			                 }
			                 finally
			                 {
				                 foreach (var s in items)
				                 {
					                 sources.Remove(s);
				                 }
								 _timers.Remove(key);
			                 }
		                 };
		var result = new ThrottleContext(timer, sources);
		_timers.Assign(key, result);
		return result;
	}
}

public sealed record ThrottleContext(Timer Subject, HashSet<TaskCompletionSource> Sources);

public class Throttling : ICommand
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
}