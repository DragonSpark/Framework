using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public interface IPlanBuilder<T> : ISelect<Array<Step<T>>, IStopAware<T>> where T : ExternalProcess;