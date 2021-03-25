using Polly;

namespace DragonSpark.Diagnostics
{
	public class Policy<T> : Model.Results.Instance<PolicyBuilder<T>>
	{
		public Policy(PolicyBuilder<T> instance) : base(instance) {}
	}

	public class Policy : Model.Results.Instance<PolicyBuilder>
	{
		public Policy(PolicyBuilder instance) : base(instance) {}
	}

}