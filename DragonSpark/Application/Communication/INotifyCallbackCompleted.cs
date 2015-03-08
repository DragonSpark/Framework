using System;

namespace DragonSpark.Application.Communication
{
	public interface INotifyCallbackCompleted
	{
		event EventHandler<CallbackResultEventArgs> Completed;
	}
}