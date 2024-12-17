using System.Net.Http;
using DragonSpark.Diagnostics;
using Polly.Extensions.Http;

namespace DragonSpark.Application.AspNet.Communication;

sealed class ConnectionBuilder : Builder<HttpResponseMessage>
{
	public static ConnectionBuilder Default { get; } = new();

	ConnectionBuilder() : base(HttpPolicyExtensions.HandleTransientHttpError()) {}
}
