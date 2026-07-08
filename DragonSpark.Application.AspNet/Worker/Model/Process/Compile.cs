using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public class Compile<T> : StopAware<T> where T : ExternalProcess
{
	protected Compile(IPlanBuilder<T> builder, params Step<T>[] steps) : base(builder.Get(steps)) {}
}