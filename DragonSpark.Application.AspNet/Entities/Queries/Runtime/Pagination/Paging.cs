namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

sealed class Paging<T> : IPaging<T>
{
	readonly IFiltered _filtered;

	public Paging(IFiltered filtered) => _filtered = filtered;

	public IPages<T> Get(PagingInput<T> parameter)
	{
		var (owner, queries, compose) = parameter;
		var previous = new QueriedPages<T>(queries, compose);
		var pages    = owner.Get(previous);
		return new ContainerAwarePages<T>(owner, pages, _filtered);
	}
}