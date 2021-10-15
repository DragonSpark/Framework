using DragonSpark.Compose;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public sealed class Sort<T> : IBody<T>
{
	public static Sort<T> Default { get; } = new Sort<T>();

	Sort() {}

	public ValueTask<IQueryable<T>> Get(ComposeInput<T> parameter)
	{
		var (input, current) = parameter;
		var queryable = !string.IsNullOrEmpty(input.OrderBy) ? current.OrderBy(input.OrderBy) : current;
		var result    = queryable.ToOperation();
		return result;
	}
}