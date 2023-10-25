using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Timers;

namespace DragonSpark.Application.Runtime;

public class ThrottleOperation<T> : IOperation<T> where T : notnull
{
	readonly ISelect<T, Timer> _timers;

	public ThrottleOperation(Func<T, Task> subject, TimeSpan interval)
		: this(new Allocated<T>(subject).Then().Structure(), interval) {}

	public ThrottleOperation(Operate<T> subject, TimeSpan interval)
		: this(subject, interval, new ConcurrentDictionary<T, Timer>()) {}

	public ThrottleOperation(Operate<T> subject, TimeSpan interval, ConcurrentDictionary<T, Timer> store)
		: this(new ConcurrentTable<T, Timer>(store,
		                                     new CreateTimer<T>(store, subject, interval.TotalMilliseconds).Get)) {}

	public ThrottleOperation(ISelect<T, Timer> timers) => _timers = timers;

	public ValueTask Get(T parameter)
	{
		var subject = _timers.Get(parameter);
		subject.Stop();
		subject.Start();
		return ValueTask.CompletedTask;
	}
}