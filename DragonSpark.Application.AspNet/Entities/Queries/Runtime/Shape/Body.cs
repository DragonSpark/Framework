namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

sealed class Body<T> : AppendedBody<T>
{
	public static Body<T> Default { get; } = new();

	Body() : base(Where<T>.Default, Sort<T>.Default) {}
}