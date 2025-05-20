using DragonSpark.Model.Operations.Selection;
using System.Threading;

namespace DragonSpark.Model.Operations.Results.Stop;

public interface IStopAware<T> : ISelecting<CancellationToken, T>;