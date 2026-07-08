using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Workers.Model;

public interface IPlanBuilder<T> : ISelect<Array<Step<T>>, IStopAware<T>> where T : ExternalProcess;