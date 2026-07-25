using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System.Collections.Concurrent;

namespace DragonSpark.Application.Runtime;

public class ThrottleOperation<T> : IOperation<T> where T : notnull
{
	readonly ISelect<T, System.Timers.Timer> _timers;

	public ThrottleOperation(Func<T, Task> subject, TimeSpan interval)
		: this(new Allocated<T>(subject).Then().Structure(), interval) {}

	public ThrottleOperation(Operate<T> subject, TimeSpan interval) : this(subject, interval, new()) {}

	public ThrottleOperation(Operate<T> subject, TimeSpan interval, ConcurrentDictionary<T, System.Timers.Timer> store)
		: this(new ConcurrentTable<T, System.Timers.Timer>(store,
		                                                   new CreateTimer<T>(store, subject,
		                                                                      interval.TotalMilliseconds).Get)) {}

	public ThrottleOperation(ISelect<T, System.Timers.Timer> timers) => _timers = timers;

	public ValueTask Get(T parameter)
	{
		var subject = _timers.Get(parameter);
		subject.Stop();
		subject.Start();
		return ValueTask.CompletedTask;
	}
}

public class ThrottleOperation : ThrottleOperation<None>, IOperation
{
	public ThrottleOperation(Func<Task> subject, TimeSpan interval) : base(_ => subject(), interval) {}

	public ThrottleOperation(Operate subject, TimeSpan interval) : base(_ => subject(), interval) {}

	public ThrottleOperation(Operate subject, TimeSpan interval, ConcurrentDictionary<None, System.Timers.Timer> store)
		: base(_ => subject(), interval, store) {}

	public ThrottleOperation(ISelect<None, System.Timers.Timer> timers) : base(timers) {}

	public ValueTask Get() => Get(None.Default);
}