using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public interface IStepBuilder<T> : ISelect<Step<T>, IStopAware<T>> where T : ExternalProcess;