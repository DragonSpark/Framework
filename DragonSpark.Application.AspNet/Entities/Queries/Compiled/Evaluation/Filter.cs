using DragonSpark.Compose;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.Json;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class Filter<T> : IFilter<T>
{
	public static Filter<T> Default { get; } = new();

	Filter() {}

	public IQueryable<T> Get(FilterInput<T> parameter)
	{
		var (queryable, filter) = parameter;
		foreach (var item in JsonSerializer.Deserialize<Filters>(filter).Verify())
		{
			queryable = queryable.Where($"{item.Property}.Contains(@0)", item.Value);
		}

		return queryable;
	}
}