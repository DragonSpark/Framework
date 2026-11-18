using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Diagnostics;

public sealed class ShouldProcess : InverseCondition<Exception>
{
	public static ShouldProcess Default { get; } = new();

	ShouldProcess() : base(IgnoredApplicationException.Default) {}
}