using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class CreateCallback : ISelect<HubConnection, IDisposable>
{
	readonly string     _method;
	readonly Func<Task> _on;

	public CreateCallback(string method, Func<Task> on)
	{
		_method = method;
		_on     = on;
	}

	public IDisposable Get(HubConnection parameter) => parameter.On(_method, _on);
}

sealed class CreateCallback<T> : ISelect<HubConnection, IDisposable>
{
	readonly string        _method;
	readonly Func<T, Task> _on;

	public CreateCallback(string method, Func<T, Task> on)
	{
		_method = method;
		_on     = on;
	}

	public IDisposable Get(HubConnection parameter) => parameter.On(_method, _on);
}