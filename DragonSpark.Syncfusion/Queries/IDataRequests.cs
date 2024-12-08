using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Model.Selection;

namespace DragonSpark.SyncfusionRendering.Queries;

public interface IDataRequests<T> : ISelect<IPages<T>, IDataRequest>;