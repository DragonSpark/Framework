using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public sealed class ProcessCheckpoint : IOperation<ProcessEventArgs>
{
	readonly Timer                           _timer;
	readonly Variable<CheckpointInformation> _store;
	readonly ITime                           _time;

	public ProcessCheckpoint()
		: this(new() { Interval = TimeSpan.FromSeconds(5).TotalMilliseconds, AutoReset = false }, new(new()),
		       Time.Default) {}

	public ProcessCheckpoint(Timer timer, Variable<CheckpointInformation> store, ITime time)
	{
		_timer         =  timer;
		_store         =  store;
		_time          =  time;
		_timer.Elapsed += TimerOnElapsed;
	}

	async void TimerOnElapsed(object? sender, ElapsedEventArgs e)
	{
		var (_, subject) = _store.Get();
		try
		{
			if (subject.HasValue)
			{
				await subject.Value.UpdateCheckpointAsync().Await();
			}
		}
		catch
		{
			// ignored
		}
		finally
		{
			_store.Execute(new(_time.Get(), subject));
		}
	}

	public async ValueTask Get(ProcessEventArgs parameter)
	{
		_timer.Stop();

		var (last, _) = _store.Get();

		var now = _time.Get();
		if (now - last >= TimeSpan.FromMinutes(1))
		{
			_store.Execute(new(now, parameter));
			await parameter.UpdateCheckpointAsync().Await();
		}
		else
		{
			_store.Execute(new(last, parameter));
			_timer.Start();
		}
	}
}