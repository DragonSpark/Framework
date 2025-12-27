using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public readonly record struct PagingInput<T>(IPageContainer<T> Owner, IQueries<T> Queries, ICompose<T> Compose)
{
	public PagingInput(IQueries<T> Queries, ICompose<T> Compose)
		: this(EmptyPageContainer<T>.Default, Queries, Compose) {}
}