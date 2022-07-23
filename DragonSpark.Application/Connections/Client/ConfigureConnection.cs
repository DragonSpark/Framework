using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Http.Connections.Client;

namespace DragonSpark.Application.Connections.Client;

sealed class ConfigureConnection : AppendedCommand<HttpConnectionOptions>, IConfigureConnection
{
	public ConfigureConnection(IConfigureConnection previous, AssignClientStateHeader header)
		: base(previous, header) {}
}