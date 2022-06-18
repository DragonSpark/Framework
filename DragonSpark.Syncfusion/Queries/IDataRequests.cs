using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Model.Selection;

namespace DragonSpark.SyncfusionRendering.Queries;

public interface IDataRequests<T> : ISelect<IPages<T>, IDataRequest> {}