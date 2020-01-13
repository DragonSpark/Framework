using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;
using Refit;

namespace DragonSpark.Server.Communication
{
	public sealed class DefaultPolicies : Result<PolicyBuilder>
	{
		public static DefaultPolicies Default { get; } = new DefaultPolicies();

		DefaultPolicies() : base(ResourcePolicies.Default.Then().Bind(Policy.Handle<ApiException>)) {}
	}
}