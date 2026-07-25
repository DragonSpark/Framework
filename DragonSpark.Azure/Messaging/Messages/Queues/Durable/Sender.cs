using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

public abstract class Sender : StopAware<MessageProperties>, ISender
{
	readonly IDispatch _dispatch;

	protected Sender(IWriteMessage write, ServiceBusConfiguration configuration, string name)
		: this(write, $"{name}{configuration.Audience}".ToLowerInvariant()) {}

	protected Sender(IWriteMessage write, string name) : this(new Dispatch(write, name)) {}

	protected Sender(IDispatch dispatch) : base(dispatch) => _dispatch = dispatch;

	public ISend Get(ScopedInput parameter)
	{
		var (visibility, life) = parameter;
		return new Send(_dispatch, visibility, life);
	}
}