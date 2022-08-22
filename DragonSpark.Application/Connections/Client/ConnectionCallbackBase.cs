using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

class ConnectionCallbackBase : ISelect<HubConnection, IDisposable>
{
	readonly IMutable<CallbackState?>            _store;
	readonly ISelect<HubConnection, IDisposable> _create;

	protected ConnectionCallbackBase(ISelect<HubConnection, IDisposable> create)
		: this(new Variable<CallbackState>(), create) {}

	protected ConnectionCallbackBase(IMutable<CallbackState?> store, ISelect<HubConnection, IDisposable> create)
	{
		_store  = store;
		_create = create;
	}

	public IDisposable Get(HubConnection parameter)
	{
		var current = _store.Get();
		if (current?.Connection != parameter)
		{
			var result = _create.Get(parameter);
			_store.Execute(new(parameter, result));
			return result;
		}

		return current.Callback;
	}
}