using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class Send : ISend
{
	readonly IDispatch _dispatch;
	readonly TimeSpan? _life, _visibility;

	public Send(IDispatch dispatch, TimeSpan? visibility = null, TimeSpan? life = null)
	{
		_dispatch   = dispatch;
		_life       = life;
		_visibility = visibility;
	}

	public ValueTask Get(Stop<MessageBody> parameter)
	{
		var (subject, stop) = parameter;
		return _dispatch.Get(new(new(subject, _visibility, _life), stop));
	}
}