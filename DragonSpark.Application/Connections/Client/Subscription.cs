using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class Subscription : SubscriptionBase
{
	public Subscription(IResult<HubConnection> connection, string method, Func<Task> on)
		: base(connection, new Callback(method, on)) {}
}

sealed class Subscription<T> : SubscriptionBase
{
	public Subscription(IResult<HubConnection> connection, string method, Func<T, Task> on)
		: base(connection, new Callback<T>(method, on)) {}
}