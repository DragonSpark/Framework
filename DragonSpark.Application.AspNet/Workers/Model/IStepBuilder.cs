using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Workers.Model;

public interface IStepBuilder<T> : ISelect<Step<T>, IStopAware<T>> where T : ExternalProcess;