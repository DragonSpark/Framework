using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Http.Connections.Client;

namespace DragonSpark.Application.Connections;

sealed class ConfigureConnection : Command<HttpConnectionOptions>, IConfigureConnection
{
	public ConfigureConnection(IAssignSignedContent command) : base(command) {}
}