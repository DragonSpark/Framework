using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

class CallbackBase : ICallback
{
	readonly IMutable<IDisposable?>              _current;
	readonly ISelect<HubConnection, IDisposable> _store;

	protected CallbackBase(ISelect<HubConnection, IDisposable> store) : this(new Variable<IDisposable>(), store) {}

	protected CallbackBase(IMutable<IDisposable?> current, ISelect<HubConnection, IDisposable> store)
	{
		_current = current;
		_store   = store;
	}

	public void Execute(HubConnection parameter)
	{
		var current = _store.Get(parameter);
		if (!ReferenceEquals(_current.Get(), current))
		{
			_current.Execute(current);
		}
	}

	public void Dispose()
	{
		_current.Get()?.Dispose();
	}
}