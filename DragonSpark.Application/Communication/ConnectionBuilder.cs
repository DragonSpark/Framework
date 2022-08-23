using DragonSpark.Diagnostics;
using Polly.Extensions.Http;
using System.Net.Http;

namespace DragonSpark.Application.Communication;

sealed class ConnectionBuilder : Builder<HttpResponseMessage>
{
	public static ConnectionBuilder Default { get; } = new ConnectionBuilder();

	ConnectionBuilder() : base(HttpPolicyExtensions.HandleTransientHttpError()) {}
}