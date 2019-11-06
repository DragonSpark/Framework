using DragonSpark.Model.Results;
using DragonSpark.Runtime.Execution;

// ReSharper disable All

namespace DragonSpark.Application.Hosting.xUnit
{
	public sealed class DefaultExecutionContext : Result<object>, IExecutionContext
	{
		public static IExecutionContext Default { get; } = new DefaultExecutionContext();

		DefaultExecutionContext() :
			base(() => new ContextDetails("xUnit Testing Application Default (root) Execution Context")) {}
	}
}