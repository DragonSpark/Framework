using DragonSpark.Application.Entities.Queries.Runtime.Shape;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class SyncfusionCompose<T> : Compose<T>
{
	public static SyncfusionCompose<T> Default { get; } = new SyncfusionCompose<T>();

	SyncfusionCompose() : base(Body<T>.Default) {}
}