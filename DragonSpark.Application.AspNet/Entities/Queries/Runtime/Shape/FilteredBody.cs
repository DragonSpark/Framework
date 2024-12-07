namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public sealed class FilteredBody<T> : AppendedBody<T>
{
	public FilteredBody(string filter) : base(new Filter<T>(filter), Sort<T>.Default) {}
}