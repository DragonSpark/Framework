using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Polly;
using System.Net.Http;

namespace DragonSpark.Application.Communication
{
	sealed class LocationAwareConnectionPolicy : Instance<PolicyBuilder<HttpResponseMessage>>
	{
		public static LocationAwareConnectionPolicy Default { get; } = new LocationAwareConnectionPolicy();

		LocationAwareConnectionPolicy()
			: base(ConnectionPolicy.Default.Then()
			                       .Select(x => x.OrResult(y => y.StatusCode == System.Net.HttpStatusCode.NotFound))
			                       .Instance()) {}
	}
}