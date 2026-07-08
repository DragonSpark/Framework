using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public class StepBuilder<T> : IStepBuilder<T> where T : ExternalProcess
{
	public IStopAware<T> Get(Step<T> parameter) => new Execute<T>(parameter.Body, parameter.Identifier);
}