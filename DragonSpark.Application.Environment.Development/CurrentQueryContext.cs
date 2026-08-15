using DragonSpark.Runtime.Execution;

namespace DragonSpark.Application.Environment.Development;

sealed class CurrentQueryContext : Logical<QueryContext>
{
	public static CurrentQueryContext Default { get; } = new();

	CurrentQueryContext() {}
}