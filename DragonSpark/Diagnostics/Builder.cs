using DragonSpark.Model.Results;
using Polly;

namespace DragonSpark.Diagnostics;

public class Builder<T> : Instance<PolicyBuilder<T>>
{
	protected Builder(PolicyBuilder<T> instance) : base(instance) {}
}

public class Builder : Instance<PolicyBuilder>
{
	protected Builder(PolicyBuilder instance) : base(instance) {}
}