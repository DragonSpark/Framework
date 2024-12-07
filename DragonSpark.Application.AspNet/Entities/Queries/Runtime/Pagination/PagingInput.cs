using DragonSpark.Application.Entities.Queries.Runtime.Shape;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public readonly record struct PagingInput<T>(IPageContainer<T> Owner, IQueries<T> Queries, ICompose<T> Compose);