namespace DragonSpark.Application.Entities.Queries.Runtime.Shape
{
	public readonly record struct PagingInput<T>(IQueries<T> Queries, ICompose<T> Compose);
}