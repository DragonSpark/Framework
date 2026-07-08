using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public class Steps<T> : Instances<Step<T>> where T : ExternalProcess
{
	protected Steps(params Step<T>[] instance) : base(instance) {}
}