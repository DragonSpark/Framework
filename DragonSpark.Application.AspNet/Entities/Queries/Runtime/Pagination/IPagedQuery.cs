using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public interface IPagedQuery<TIn, TOut> : IStopAware<PageQueryInput<TIn>, PageResult<TOut>>;