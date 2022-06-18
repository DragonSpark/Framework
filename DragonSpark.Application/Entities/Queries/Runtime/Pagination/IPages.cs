using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public interface IPages<T> : ISelecting<PageInput, Page<T>> {}