using DragonSpark.Model.Results;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;

namespace DragonSpark.Application.Communication
{
	sealed class ConnectionPolicy : Instance<PolicyBuilder<HttpResponseMessage>>
	{
		public static ConnectionPolicy Default { get; } = new ConnectionPolicy();

		ConnectionPolicy() : base(HttpPolicyExtensions.HandleTransientHttpError()) {}
	}
}