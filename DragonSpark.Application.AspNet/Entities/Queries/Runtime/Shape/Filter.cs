using DragonSpark.Compose;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public sealed class Filter<T> : IBody<T>
{
	readonly string _filter;

	public Filter(string filter) => _filter = filter;

	public ValueTask<IQueryable<T>> Get(ComposeInput<T> parameter)
	{
		var (input, current) = parameter;
		var queryable = input.Filter != null ? current.Where(_filter, input.Filter) : current;
		var result    = queryable.ToOperation();
		return result;
	}
}