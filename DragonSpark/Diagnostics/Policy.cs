using Polly;

namespace DragonSpark.Diagnostics
{
	public class Policy<T> : Model.Results.Instance<PolicyBuilder<T>>
	{
		protected Policy(PolicyBuilder<T> instance) : base(instance) {}
	}

	public class Policy : Model.Results.Instance<PolicyBuilder>
	{
		protected Policy(PolicyBuilder instance) : base(instance) {}
	}

}