using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public class Steps<T> : Instances<Step<T>> where T : ExternalProcess
{
	protected Steps(params Step<T>[] instance) : base(instance) {}
}