using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

public abstract class DurableSender : StopAware<MessageProperties>, IDurableSender
{
	readonly IDispatch _dispatch;

	protected DurableSender(IWriteMessage write, ServiceBusConfiguration configuration, string name)
		: this(write, $"{name}{configuration.Audience}".ToLowerInvariant()) {}

	protected DurableSender(IWriteMessage write, string name) : this(new Dispatch(write, name)) {}

	protected DurableSender(IDispatch dispatch) : base(dispatch) => _dispatch = dispatch;

	public IScopedDispatch Get(ScopedInput parameter)
	{
		var (visibility, life) = parameter;
		return new ScopedDispatch(_dispatch, visibility, life);
	}
}