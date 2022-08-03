using DragonSpark.Model.Selection.Stores;
using System;
using System.Timers;

namespace DragonSpark.Application.Runtime;

public class ProtectionAwareThrottling<T> : IThrottling<T> where T : notnull
{
	readonly IThrottling<T> _previous;

	protected ProtectionAwareThrottling(ITable<T, Timer> timers) : this(timers, TimeSpan.FromMilliseconds(750)) {}

	protected ProtectionAwareThrottling(ITable<T, Timer> timers, TimeSpan window)
		: this(new Throttling<T>(timers, window)) {}

	public ProtectionAwareThrottling(IThrottling<T> previous) => _previous = previous;

	public void Execute(Throttle<T> parameter)
	{
		var (_, method) = parameter;
		lock (method.Method)
		{
			_previous.Execute(parameter);
		}
	}
}