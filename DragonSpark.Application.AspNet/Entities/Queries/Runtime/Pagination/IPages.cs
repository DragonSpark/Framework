using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public interface IPages<T> : ISelecting<PageInput, Page<T>>;