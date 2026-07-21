using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ScopedDispatch : IScopedDispatch
{
	readonly IDispatch _dispatch;
	readonly TimeSpan? _life, _visibility;

	public ScopedDispatch(IDispatch dispatch, TimeSpan? visibility = null, TimeSpan? life = null)
	{
		_dispatch   = dispatch;
		_life       = life;
		_visibility = visibility;
	}

	public ValueTask Get(Stop<IdentifiedMessage> parameter)
	{
		var (subject, stop) = parameter;
		return _dispatch.Get(new(new(subject, _visibility, _life), stop));
	}
}