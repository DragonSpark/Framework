using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Model.Selection;

namespace DragonSpark.Syncfusion.Queries;

public interface IDataRequests<T> : ISelect<IPaging<T>, IDataRequest> {}