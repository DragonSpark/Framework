using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

public class Subscribe : ISubscribe
{
	readonly IResult<HubConnection> _connection;
	readonly string    _name;

	protected Subscribe(IResult<HubConnection> connection, string name)
	{
		_connection = connection;
		_name       = name;
	}

	public ISubscription Get(Func<Task> parameter)
		=> new PolicyAwareSubscription(new Subscription(_connection, _name, parameter));
}