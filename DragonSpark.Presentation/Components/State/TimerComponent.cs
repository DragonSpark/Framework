using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace DragonSpark.Presentation.Components.State;

public class TimerComponent : Microsoft.AspNetCore.Components.ComponentBase, IDisposable
{
	Func<Task> _refresh = default!;

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
					UpdateRequested =  true;
				}
			}
		}
	}	Timer? _timer;

	[Parameter]
	public bool AutoStart
	{
		get => _autoStart;
		set
		{
			if (_autoStart != value)
			{
				_autoStart      =  value;
				UpdateRequested |= _autoStart;
			}
		}
	}	bool _autoStart;

	[Parameter]
	public bool Enabled
	{
		get => _enabled;
		set
		{
			if (_enabled != value)
			{
				_enabled        =  value;
				UpdateRequested |= _enabled;
			}
		}
	}	bool _enabled = true;

	[Parameter]
	public bool Repeat { get; set; }

	[Parameter]
	public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(1);

	[Parameter]
	public EventCallback Updated { get; set; }

	bool UpdateRequested { get; set; } = true;

	protected override void OnInitialized()
	{
		base.OnInitialized();
		_refresh = () => Updated.InvokeAsync();
		Timer    = new Timer();
	}

	protected override void OnParametersSet()
	{
		if (UpdateRequested && !(UpdateRequested = false) && _timer is not null)
		{
			_timer.Stop();
			if (Enabled)
			{
				_timer.AutoReset = Repeat;
				_timer.Interval  = Interval.TotalMilliseconds;
				_timer.Start();
			}
		}

		base.OnParametersSet();
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