using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;

namespace DragonSpark.Application.Connections.Client;

sealed class ConnectionAware : MutationAware<HubConnection>
{
	public ConnectionAware(ConnectionVariable store) : base(store, new ValidConnection(store)) {}
}