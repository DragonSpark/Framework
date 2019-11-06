using DragonSpark.Model.Results;

namespace DragonSpark.Runtime.Execution
{
	public sealed class ExecutionContext : Result<object>, IExecutionContext
	{
		public static ExecutionContext Default { get; } = new ExecutionContext();

		ExecutionContext() : base(ExecutionContextStore.Default) {}
	}
}