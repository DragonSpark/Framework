using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class ConnectionVariable : IMutable<HubConnection?>, IDisposable
{
	readonly IMutable<HubConnection?> _previous;

	public ConnectionVariable() : this(new Variable<HubConnection>()) {}

	public ConnectionVariable(IMutable<HubConnection?> previous) => _previous = previous;

	public HubConnection? Get() => _previous.Get();

	public void Execute(HubConnection? parameter)
	{
		var current = _previous.Get();
		if (current is not null && current != parameter)
		{
			Dispose();
		}
		_previous.Execute(parameter);
	}

	public void Dispose()
	{
		var current = _previous.Get();
		_previous.Execute(default);
		if (current is not null)
		{
			Task.Run(() => current.DisposeAsync());
		}
	}
}