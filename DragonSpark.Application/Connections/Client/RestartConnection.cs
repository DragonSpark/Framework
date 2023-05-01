using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class RestartConnection : IRestartConnection
{
	public static RestartConnection Default { get; } = new();

	RestartConnection() {}

	public async ValueTask Get(HubConnection parameter)
	{
		await parameter.StopAsync().ConfigureAwait(false);
		await parameter.StartAsync().ConfigureAwait(false);
	}
}