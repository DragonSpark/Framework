using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public sealed class EmptyPaging<T> : Instance<IPages<T>>
{
	public static EmptyPaging<T> Default { get; } = new();

	EmptyPaging() : base(EmptyPages<T>.Default) {}
}