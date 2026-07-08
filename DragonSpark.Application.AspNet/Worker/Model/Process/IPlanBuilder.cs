using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public interface IPlanBuilder<T> : ISelect<Array<Step<T>>, IStopAware<T>> where T : ExternalProcess;