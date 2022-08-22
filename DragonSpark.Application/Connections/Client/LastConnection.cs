using DragonSpark.Application.Diagnostics.Time;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Application.Connections.Client;

sealed class LastConnection : ICondition
{
	readonly IMutable<DateTimeOffset?> _store;
	readonly IWindow                   _window;
	readonly ITime                     _time;

	public LastConnection() : this(Time.Default) {}

	public LastConnection(ITime time)
		: this(new Variable<DateTimeOffset?>(), time.IsPast(TimeSpan.FromDays(-.9)), time) {}

	public LastConnection(IMutable<DateTimeOffset?> store, IWindow window, ITime time)
	{
		_store  = store;
		_window = window;
		_time   = time;
	}

	public bool Get(None parameter)
	{
		var current = _store.Get();
		if (current is null || _window.Get(current.Value))
		{
			_store.Execute(_time.Get());
			return current is null;
		}

		return true;
	}
}