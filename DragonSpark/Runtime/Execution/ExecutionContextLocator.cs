using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Runtime.Execution
{
	sealed class ExecutionContextLocator : Result<IExecutionContext>
	{
		public static ExecutionContextLocator Default { get; } = new ExecutionContextLocator();

		ExecutionContextLocator()
			: base(Start.An.Instance(ComponentTypesDefinition.Default)
			            .Select(x => x.Query().FirstAssigned())
			            .Assume()
			            .Select(Activator.Default.Assigned())
			            .Then()
			            .CastForResult<IExecutionContext>()
			            .Get()
			            .To(Start.An.Extent<FixedActivator<IExecutionContext>>())
			      ) {}
	}
}