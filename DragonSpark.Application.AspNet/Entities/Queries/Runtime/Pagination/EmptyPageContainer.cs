using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public sealed class EmptyPageContainer<T> : Instance<Type>, IPageContainer<T>
{
	public static EmptyPageContainer<T> Default { get; } = new();

	EmptyPageContainer() : base(A.Type<EmptyPageContainer<T>>()) {}

	public IPages<T> Get(IPages<T> parameter) => parameter;

	public ValueTask Get(PageResult<T> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Exception parameter) => ValueTask.CompletedTask;
}