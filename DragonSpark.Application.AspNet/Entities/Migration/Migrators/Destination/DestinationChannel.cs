using DragonSpark.Model.Results;
using DragonSpark.Runtime.Invocation;
using System.Threading.Channels;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class DestinationChannel<T> : IResult<Channel<T>>
{
	public static DestinationChannel<T> Default { get; } = new();

	DestinationChannel() : this(MaximumParallelism.Default) {}

	readonly ushort _parallelism;

	public DestinationChannel(ushort parallelism) => _parallelism = parallelism;

	public Channel<T> Get() => Channel.CreateBounded<T>(new BoundedChannelOptions(_parallelism)
	{
		SingleReader = true,
		FullMode     = BoundedChannelFullMode.Wait
	});
}