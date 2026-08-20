using DragonSpark.Server.Output;
using DragonSpark.Text;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public class ClearOutputsRegistration<T> : EventRegistration<T> where T : Message
{
	protected ClearOutputsRegistration(IOutputCacheStore output, IFormatter<T> key)
		: base(new ClearOutputs<T>(output, key)) {}
	protected ClearOutputsRegistration(IOutputCacheStore output, IOutputKey key)
		: base(new ClearOutputAdapter<T>(output, key)) {}
}

public class ClearOutputsRegistration<TIn, T> : EventRegistration<T, TIn> where T : Message<TIn>
{
	protected ClearOutputsRegistration(IOutputCacheStore output, IFormatter<TIn> key)
		: base(new ClearOutputs<TIn>(output, key)) {}
}