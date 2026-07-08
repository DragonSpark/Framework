using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public interface IStepBuilder<T> : ISelect<Step<T>, IStopAware<T>> where T : ExternalProcess;