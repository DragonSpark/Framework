using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Timers;
using ValueTask = System.Threading.Tasks.ValueTask;

namespace DragonSpark.Application.Runtime
{
	public class Throttling<T> : IThrottling<T>
	{
		readonly StripedAsyncLock<T> _lock;
		readonly ITable<T, Timer>    _timers;
		readonly TimeSpan            _duration;

		public Throttling(ITable<T, Timer> timers)
			: this(new StripedAsyncLock<T>(16), timers, TimeSpan.FromMilliseconds(750)) {}

		public Throttling(StripedAsyncLock<T> @lock, ITable<T, Timer> timers, TimeSpan duration)
		{
			_lock     = @lock;
			_timers   = timers;
			_duration = duration;
		}

		public async ValueTask Get(Throttle<T> parameter)
		{
			var (input, action) = parameter;
			using var @lock  = await _lock.LockAsync(input);
			var       result = _timers.Get(input).Account();
			if (result == null)
			{
				result = new Timer { Enabled = false, AutoReset = false, Interval = _duration.TotalMilliseconds };
				result.Elapsed += (_, _) => action(input);
				_timers.Assign(input, result);
			}

			result.Stop();
			result.Start();
		}
	}
}