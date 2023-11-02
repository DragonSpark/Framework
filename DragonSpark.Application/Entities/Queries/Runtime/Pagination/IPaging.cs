using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public interface IPaging<T> : ISelect<PagingInput<T>, IPages<T>>;