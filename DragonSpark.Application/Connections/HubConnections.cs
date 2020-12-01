using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections
{
	sealed class HubConnections : IHubConnections
	{
		readonly Action<HttpConnectionOptions> _configure;

		public HubConnections(IConfigureConnection configure) : this(configure.Execute) {}

		public HubConnections(Action<HttpConnectionOptions> configure) => _configure = configure;

		public HubConnection Get(Uri parameter)
			=> new HubConnectionBuilder().WithUrl(parameter, _configure).WithAutomaticReconnect().Build();
	}
}