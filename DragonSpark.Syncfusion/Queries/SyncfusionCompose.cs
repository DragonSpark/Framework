using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class SyncfusionCompose<T> : Compose<T>
{
	public static SyncfusionCompose<T> Default { get; } = new();

	SyncfusionCompose() : base(Body<T>.Default) {}
}