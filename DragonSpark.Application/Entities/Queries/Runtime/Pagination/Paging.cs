namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

sealed class Paging<T> : IPaging<T>
{
	public static Paging<T> Default { get; } = new();

	Paging() {}

	public IPages<T> Get(PagingInput<T> parameter) => new Pages<T>(parameter.Queries, parameter.Compose);
}