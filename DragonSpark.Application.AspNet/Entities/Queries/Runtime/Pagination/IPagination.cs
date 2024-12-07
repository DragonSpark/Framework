using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public interface IPagination<T> : IAlteration<IPages<T>>;