using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public interface IPagination<T> : ISelecting<PagingInput<T>, IPages<T>> {}