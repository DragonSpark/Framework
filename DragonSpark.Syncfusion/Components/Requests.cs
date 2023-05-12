using DragonSpark.SyncfusionRendering.Queries;

namespace DragonSpark.SyncfusionRendering.Components;

sealed class Requests : IRequests
{
	public static Requests Default { get; } = new();

	Requests() {}

	public IDataRequest Get(IDataRequest parameter) => parameter;
}