using DragonSpark.Runtime.Execution;
using System;

namespace DragonSpark.Composition.Scopes;

public sealed class LogicalProvider : Logical<IServiceProvider>
{
	public static LogicalProvider Default { get; } = new();

	LogicalProvider() {}
}