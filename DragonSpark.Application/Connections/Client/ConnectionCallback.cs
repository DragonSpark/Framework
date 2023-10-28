using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class ConnectionCallback : ConnectionCallbackBase
{
	public ConnectionCallback(string method, Func<Task> on) : base(new CreateCallback(method, on)) {}
}

sealed class ConnectionCallback<T> : ConnectionCallbackBase
{
	public ConnectionCallback(string method, Func<T, Task> on) : base(new CreateCallback<T>(method, on)) {}
}