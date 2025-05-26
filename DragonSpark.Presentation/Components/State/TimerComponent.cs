using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace DragonSpark.Presentation.Components.State;

public class TimerComponent : Microsoft.AspNetCore.Components.ComponentBase, IDisposable
{
	readonly Switch _update  = true;
	Func<Task>      _refresh = null!;

	[Parameter]
	public Timer? Timer
	{
		get => _timer;
		set
		{
			if (_timer != value)
			{
				if (_timer is not null)
				{
					_timer.Elapsed -= OnElapsed;
					_timer.Stop();
				}

				if ((_timer = value) is not null)
				{
					_timer.Elapsed  += OnElapsed;
					_update.Up();
				}
			}
		}
	}	Timer? _timer;

	[Parameter]
	public bool AutoStart
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				_update.Execute(_update || field);
			}
		}
	}

	[Parameter]
	public bool Enabled
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				_update.Execute(_update || field);
			}
		}
	} = true;

	[Parameter]
	public bool Repeat { get; set; }

	[Parameter]
	public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(1);

	[Parameter]
	public EventCallback Updated { get; set; }

	protected override void OnInitialized()
	{
		base.OnInitialized();
		_refresh = () => Updated.Invoke();
		Timer    = new Timer();
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (_update.Down() && _timer is not null)
		{
			_timer.Stop();
			if (Enabled)
			{
				_timer.AutoReset = Repeat;
				_timer.Interval  = Interval.TotalMilliseconds;
				_timer.Start();
			}
		}
		base.OnAfterRender(firstRender);
	}

	void OnElapsed(object? sender, ElapsedEventArgs e)
	{
		InvokeAsync(_refresh);
	}

	public void Dispose()
	{
		_timer?.Stop();
		_timer?.Dispose();
	}
}