namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public sealed class DefaultCompose<T> : Compose<T>
{
	public static DefaultCompose<T> Default { get; } = new();

	DefaultCompose() : base(Body<T>.Default) {}
}