using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Workers.Model;

public class Steps<T> : Instances<Step<T>> where T : ExternalProcess
{
	protected Steps(params Step<T>[] instance) : base(instance) {}
}