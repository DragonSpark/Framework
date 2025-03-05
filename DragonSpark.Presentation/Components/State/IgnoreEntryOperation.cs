using System;
using System.Threading.Tasks;
using System.Timers;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime.Execution;

namespace DragonSpark.Presentation.Components.State;

sealed class IgnoreEntryOperation : IOperation, ICommand
{
	readonly IOperation _operation;
	readonly FirstBase  _active;
	readonly Timer      _timer;
	readonly TimeSpan   _duration;

	public IgnoreEntryOperation(IOperation operation) : this(operation, TimeSpan.FromSeconds(1)) {}

	public IgnoreEntryOperation(IOperation operation, TimeSpan duration) : this(operation, new First(), duration) {}

	public IgnoreEntryOperation(IOperation operation, FirstBase active, TimeSpan duration)
		: this(operation, active, duration,
		       new Timer { AutoReset = false, Enabled = true, Interval = duration.TotalMilliseconds }) {}

	// ReSharper disable once TooManyDependencies
	public IgnoreEntryOperation(IOperation operation, FirstBase active, TimeSpan duration, Timer timer)
	{
		_operation     =  operation;
		_active        =  active;
		_duration      =  duration;
		_timer         =  timer;
		_timer.Elapsed += TimerOnElapsed;
	}

	void TimerOnElapsed(object? sender, ElapsedEventArgs e)
	{
		_active.Execute();
	}

	public async ValueTask Get()
	{
		if (_active.Get())
		{
			_timer.Stop();
			var captured = DateTimeOffset.Now;

			try
			{
				await _operation.Go();
			}
			finally
			{
				var elapsed = DateTimeOffset.Now - captured;
				if (elapsed < _duration)
				{
					_timer.Interval = (_duration - elapsed).TotalMilliseconds;
					_timer.Start();
				}
				else
				{
					_active.Execute();
				}
			}
		}
	}

	public void Execute(None parameter)
	{
		_timer.Stop();
		_active.Execute();
	}
}