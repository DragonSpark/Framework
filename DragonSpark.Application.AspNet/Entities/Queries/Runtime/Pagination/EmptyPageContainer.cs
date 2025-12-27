using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public sealed class EmptyPageContainer<T> : Instance<Type>, IPageContainer<T>
{
	public static EmptyPageContainer<T> Default { get; } = new();

	EmptyPageContainer() : base(A.Type<EmptyPageContainer<T>>()) {}

	public void Execute(Page<T> parameter) {}

	public void Execute(Exception parameter) {}

	public IPages<T> Get(IPages<T> parameter) => parameter;
}