using DragonSpark.Diagnostics;
using Polly.Extensions.Http;
using System.Net.Http;

namespace DragonSpark.Application.Communication
{
	sealed class ConnectionPolicy : Policy<HttpResponseMessage>
	{
		public static ConnectionPolicy Default { get; } = new ConnectionPolicy();

		ConnectionPolicy() : base(HttpPolicyExtensions.HandleTransientHttpError()) {}
	}
}