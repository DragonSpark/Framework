using DragonSpark.Compose;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public sealed class Where<T> : IBody<T>
{
	public static Where<T> Default { get; } = new();

	Where() {}

	public ValueTask<IQueryable<T>> Get(ComposeInput<T> parameter)
	{
		var (input, current) = parameter;
		var queryable = !string.IsNullOrEmpty(input.Filter) ? current.Where(input.Filter) : current;
		var result    = queryable.ToOperation();
		return result;
	}
}