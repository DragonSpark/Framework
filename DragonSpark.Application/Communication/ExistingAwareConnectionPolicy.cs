using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Polly;
using System.Net.Http;

namespace DragonSpark.Application.Communication;

sealed class ExistingAwareConnectionPolicy : Instance<PolicyBuilder<HttpResponseMessage>>
{
	public static ExistingAwareConnectionPolicy Default { get; } = new ExistingAwareConnectionPolicy();

	ExistingAwareConnectionPolicy()
		: base(ConnectionBuilder.Default.Then()
		                       .Select(x => x.OrResult(y => y.StatusCode == System.Net.HttpStatusCode.NotFound))
		                       .Instance()) {}
}