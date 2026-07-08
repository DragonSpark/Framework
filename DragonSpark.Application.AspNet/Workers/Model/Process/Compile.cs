using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public class Compile<T> : StopAware<T> where T : ExternalProcess
{
	protected Compile(IPlanBuilder<T> builder, params Step<T>[] steps) : base(builder.Get(steps)) {}
}