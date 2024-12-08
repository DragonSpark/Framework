namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

sealed class Paging<T> : IPaging<T>
{
	public static Paging<T> Default { get; } = new();

	Paging() {}

	public IPages<T> Get(PagingInput<T> parameter)
	{
		var (owner, queries, compose) = parameter;

		var inner = new QueriedPages<T>(queries, compose);
		return new ContainerAwarePages<T>(owner, inner);
	}
}

