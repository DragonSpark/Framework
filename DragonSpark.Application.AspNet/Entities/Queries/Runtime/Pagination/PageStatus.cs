using DragonSpark.Contracts.Queries;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public readonly record struct PageStatus<T>(PageResult<T> Subject, bool Filtered);