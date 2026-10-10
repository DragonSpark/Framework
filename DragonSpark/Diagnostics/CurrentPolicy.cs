using DragonSpark.Runtime.Execution;

namespace DragonSpark.Diagnostics;

sealed class CurrentPolicy : Logical<bool>
{
	public static CurrentPolicy Default { get; } = new();

	CurrentPolicy() {}
}