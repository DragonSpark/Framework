using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

public interface IPagedQuery<TIn, TOut> : IStopAware<PageQueryInput<TIn>, PageResult<TOut>>;