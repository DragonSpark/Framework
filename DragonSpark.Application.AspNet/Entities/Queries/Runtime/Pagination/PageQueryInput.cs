namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public readonly record struct PageQueryInput<T>(T Parameter, PageInput Input);