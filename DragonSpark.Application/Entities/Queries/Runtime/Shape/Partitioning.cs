using DragonSpark.Compose;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

sealed class Partitioning<T> : IPartition<T>
{
	public static Partitioning<T> Default { get; } = new Partitioning<T>();

	Partitioning() {}

	public ValueTask<IQueryable<T>> Get(Partition<T> parameter)
	{
		var (queryable, (skip, top)) = parameter;
		var first  = skip.HasValue ? queryable.Skip(skip.Value) : queryable;
		var result = top.HasValue ? first.Take(top.Value) : first;
		return result.ToOperation();
	}
}