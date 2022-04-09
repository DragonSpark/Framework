using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class DataRequests<T> : ReferenceValueStore<IPaging<T>, IDataRequest>, IDataRequests<T>
{
	public static DataRequests<T> Default { get; } = new DataRequests<T>();

	DataRequests() : base(x => new ProcessRequest<T>(x)) {}
}