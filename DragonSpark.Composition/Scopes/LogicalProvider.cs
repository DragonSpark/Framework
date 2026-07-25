using DragonSpark.Runtime.Execution;

namespace DragonSpark.Composition.Scopes;

public sealed class LogicalProvider : Logical<IServiceProvider>
{
	public static LogicalProvider Default { get; } = new();

	LogicalProvider() {}
}