using DragonSpark.Compose;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public sealed class Sort<T> : IBody<T>
{
	public static Sort<T> Default { get; } = new();

	Sort() {}

	public ValueTask<IQueryable<T>> Get(ComposeInput<T> parameter)
	{
		var (input, current) = parameter;
		var queryable = !string.IsNullOrEmpty(input.OrderBy) ? current.OrderBy(input.OrderBy) : current;
		var result    = queryable.ToOperation();
		return result;
	}
}