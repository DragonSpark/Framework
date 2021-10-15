using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Http.Connections.Client;

namespace DragonSpark.Application.Connections;

public sealed class DefaultConfigureConnection : Command<HttpConnectionOptions>, IConfigureConnection
{
	public static DefaultConfigureConnection Default { get; } = new DefaultConfigureConnection();

	DefaultConfigureConnection() : base(_ => {}) {}
}