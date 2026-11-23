using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class EmptySweep : ISweep
{
	public static EmptySweep Default { get; } = new();

	EmptySweep() {}

	public ValueTask<bool> Get(Stop<None> parameter) => false.ToOperation();
}