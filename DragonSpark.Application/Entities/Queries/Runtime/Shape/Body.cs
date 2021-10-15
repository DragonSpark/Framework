namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

sealed class Body<T> : AppendedBody<T>
{
	public static Body<T> Default { get; } = new Body<T>();

	Body() : base(Where<T>.Default, Sort<T>.Default) {}
}