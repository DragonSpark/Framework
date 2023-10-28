using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class ConnectionVariable : Result<HubConnection?>, IMutable<HubConnection?>, IAsyncDisposable
{
	readonly IMutable<HubConnection?> _previous;

	public ConnectionVariable() : this(new Variable<HubConnection>()) {}

	public ConnectionVariable(IMutable<HubConnection?> previous) : base(previous) => _previous = previous;

	public void Execute(HubConnection? parameter)
	{
		var current = _previous.Get();
		if (current is not null && current != parameter)
		{
			Clear();
		}

		_previous.Execute(parameter);
	}

	void Clear()
	{
		var dispose = DisposeAsync();
		if (!dispose.IsCompleted)
		{
			Task.Run(dispose.AsTask);
		}
	}

	public ValueTask DisposeAsync()
	{
		var current = _previous.Get();
		_previous.Execute(default);
		return current?.DisposeAsync() ?? ValueTask.CompletedTask;
	}
}