using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Runtime.Execution
{
	sealed class ExecutionContextStore : SystemStore<object>
	{
		public static ExecutionContextStore Default { get; } = new ExecutionContextStore();

		ExecutionContextStore() : this(ExecutionContextLocator.Default) {}

		public ExecutionContextStore(IResult<IResult<object>> result)
			: base(Start.A.Result(() => new ContextDetails("Default Execution Context"))
			            .Start()
			            .Unless(result)
			            .Then()
			            .Value()
			            .Selector()) {}
	}
}