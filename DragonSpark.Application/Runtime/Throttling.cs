using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace DragonSpark.Application.Runtime;

public class ThrottleOperation<T> : IOperation<T> where T : notnull
{
	readonly ISelect<T, Timer> _timers;

	public ThrottleOperation(Operate<T> subject, TimeSpan interval)
		: this(subject, interval, new Dictionary<T, Timer>()) {}

	public ThrottleOperation(Operate<T> subject, TimeSpan interval, IDictionary<T, Timer> store)
		: this(new StandardTable<T, Timer>(store, new CreateTimer<T>(store, subject, interval.TotalMilliseconds).Get)) {}

	public ThrottleOperation(ISelect<T, Timer> timers) => _timers = timers;

	public ValueTask Get(T parameter)
	{
		var subject = _timers.Get(parameter);
		subject.Stop();
		subject.Start();
		return ValueTask.CompletedTask;
	}
}
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