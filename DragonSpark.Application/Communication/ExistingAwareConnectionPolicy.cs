using DragonSpark.Compose;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Polly;
using System.Net.Http;

namespace DragonSpark.Application.Communication;

sealed class ExistingAwareConnectionPolicy : Instance<PolicyBuilder<HttpResponseMessage>>
{
	[UsedImplicitly]
	public static ExistingAwareConnectionPolicy Default { get; } = new();

	ExistingAwareConnectionPolicy()
		: base(ConnectionBuilder.Default.Then()
		                       .Select(x => x.OrResult(y => y.StatusCode == System.Net.HttpStatusCode.NotFound))
		                       .Instance()) {}
}