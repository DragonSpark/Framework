using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class Subscription : SubscriptionBase
{
	public Subscription(HubConnection connection, string method, Func<Task> on)
		: base(connection, connection.On(method, on)) {}
}

sealed class Subscription<T> : SubscriptionBase
{
	public Subscription(HubConnection connection, string method, Func<T, Task> on)
		: base(connection, connection.On(method, on)) {}
}