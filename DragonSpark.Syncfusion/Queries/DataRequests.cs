using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class DataRequests<T> : ReferenceValueStore<IPages<T>, IDataRequest>, IDataRequests<T>
{
	public static DataRequests<T> Default { get; } = new();

	DataRequests() : base(x => new ProcessRequest<T>(x)) {}
}