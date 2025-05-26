using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public class UserEventRegistration<T> : EventRegistration<T, uint> where T : NumberMessage
{
	protected UserEventRegistration(IStopAware<uint> body) : base(body) {}
}