using DragonSpark.Runtime.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Scopes;

public sealed class LogicalScope : Logical<AsyncServiceScope?>
{
	public static LogicalScope Default { get; } = new();

	LogicalScope() {}
}