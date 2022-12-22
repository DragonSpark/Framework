using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace DragonSpark.Application.Runtime;

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