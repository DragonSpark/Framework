using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Diagnostics;

public sealed class IsPolicy : Condition
{
	public static IsPolicy Default { get; } = new();

	IsPolicy() : base(A.Result(CurrentPolicy.Default).Then().Accept()) {}
}