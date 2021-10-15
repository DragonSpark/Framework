using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public class Compose<T> : ICompose<T>
{
	readonly IBody<T>       _body;
	readonly ILargeCount<T> _count;
	readonly IPartition<T>  _partition;

	public Compose(IBody<T> body) : this(body, DefaultLargeCount<T>.Default, Partitioning<T>.Default) {}

	public Compose(IBody<T> body, ILargeCount<T> count, IPartition<T> partition)
	{
		_body      = body;
		_count     = count;
		_partition = partition;
		_partition = partition;
	}

	public async ValueTask<Composition<T>> Get(ComposeInput<T> parameter)
	{
		var (input, _) = parameter;
		var body      = await _body.Await(parameter);
		var count     = input.IncludeTotalCount ? await _count.Await(body) : default(ulong?);
		var partition = input.Partition.HasValue ? await _partition.Await(new(body, input.Partition.Value)) : body;
		return new(partition, count);
	}
}