using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Diagnostics;

public sealed class ShouldProcess : Condition<Exception>
{
	public static ShouldProcess Default { get; } = new();

	ShouldProcess() : base(AggregateAwareIgnoreException.Default.Then().Inverse()) {}
}