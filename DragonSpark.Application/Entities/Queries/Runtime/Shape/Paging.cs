namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public sealed class Paging<T> : IPaging<T>
{
	public static Paging<T> Default { get; } = new Paging<T>();

	Paging() {}

	public IPages<T> Get(PagingInput<T> parameter) => new Pages<T>(parameter.Queries, parameter.Compose);
}