using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Workers.Model;

public class StepBuilder<T> : IStepBuilder<T> where T : ExternalProcess
{
	public IStopAware<T> Get(Step<T> parameter) => new Execute<T>(parameter.Body, parameter.Identifier);
}