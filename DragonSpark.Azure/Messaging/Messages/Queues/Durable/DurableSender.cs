using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

public abstract class DurableSender : StopAware<MessageProperties>, IDurableSender
{
	readonly IDispatch _dispatch;

	protected DurableSender(string name, ServiceBusConfiguration configuration)
		: this(name, configuration.Audience) {}

	protected DurableSender(string name, string? audience) : this(new Dispatch(name, audience)) {}

	protected DurableSender(IDispatch dispatch) : base(dispatch) => _dispatch = dispatch;

	public IScopedDispatch Get(ScopedInput parameter)
	{
		var (visibility, life) = parameter;
		return new ScopedDispatch(_dispatch, visibility, life);
	}
}