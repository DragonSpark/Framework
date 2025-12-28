using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public interface IPages<T> : IStopAware<PageInput, Page<T>>;