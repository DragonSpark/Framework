using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Compose.Entities.Queries.Composition.Runtime;

public sealed class ComposeComposer<T> : IResult<ICompose<T>>
{
	readonly IBody<T>       _body;
	readonly ILargeCount<T> _count;
	readonly IPartition<T>  _partition;

	public ComposeComposer() : this(Body<T>.Default, DefaultLargeCount<T>.Default, Partitioning<T>.Default) {}

	public ComposeComposer(IBody<T> body, ILargeCount<T> count, IPartition<T> partition)
	{
		_body      = body;
		_count     = count;
		_partition = partition;
	}

	public ComposeComposer<T> Filter(string filter)
		=> new (new AppendedBody<T>(new Filter<T>(filter), _body), _count, _partition);

	public ICompose<T> Get() => new Compose<T>(_body, _count, _partition);
}