using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class RestartConnection : IRestartConnection
{
	public static RestartConnection Default { get; } = new();

	RestartConnection() {}

	public async ValueTask Get(IReceiveConnection parameter)
	{
		var current = parameter.Get();
		await parameter.DisposeAsync().ConfigureAwait(false);
		var next = parameter.Get();
		if (current == next)
		{
			throw new InvalidOperationException("Expected a new connection, but one was not found");
		}
		await next.StartAsync().ConfigureAwait(false);
	}
}