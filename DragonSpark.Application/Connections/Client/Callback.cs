using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class Callback : CallbackBase
{
	public Callback(string method, Func<Task> on) : base(new ConnectionCallback(method, on)) {}
}

sealed class Callback<T> : CallbackBase
{
	public Callback(string method, Func<T, Task> on) : base(new ConnectionCallback<T>(method, on)) {}
}