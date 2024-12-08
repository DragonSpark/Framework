namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

sealed class PolicyAwarePaging<T> : IPaging<T>
{
	readonly IPaging<T> _previous;

	public PolicyAwarePaging(IPaging<T> previous) => _previous = previous;

	public IPages<T> Get(PagingInput<T> parameter) => new PolicyAwarePages<T>(_previous.Get(parameter));
}