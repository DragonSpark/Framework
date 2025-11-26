using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class ActivityAwareOperation : IOperation
{
	readonly IOperation            _operation;
	readonly IActivityReceiver     _subject;
	readonly ActivityReceiverState _state;

	public ActivityAwareOperation(IOperation operation, IActivityReceiver subject)
		: this(operation, subject, ActivityOptions.Default) {}

	public ActivityAwareOperation(IOperation operation, IActivityReceiver subject, ActivityOptions options)
		: this(operation, subject, new ActivityReceiverState(operation, options)) {}

	public ActivityAwareOperation(IOperation operation, IActivityReceiver subject, ActivityReceiverState state)
	{
		_operation = operation;
		_subject   = subject;
		_state     = state;
	}

	public async ValueTask Get()
	{
		var operation = _operation.Get();
		if (!operation.IsCompleted)
		{
			await _subject.On(_state);
			try
			{
				await operation.On();
			}
			finally
			{
				await _subject.Off();
			}
		}
	}
}

sealed class ActivityAwareOperation<T> : IOperation<T>
{
	readonly IOperation<T>           _operation;
	readonly IActivityReceiver       _subject;
	readonly ActivityReceiverState   _state;

	public ActivityAwareOperation(IOperation<T> operation, IActivityReceiver subject)
		: this(operation, subject, ActivityOptions.Default) {}

	public ActivityAwareOperation(IOperation<T> operation, IActivityReceiver subject, ActivityOptions options)
		: this(operation, subject, new ActivityReceiverState(operation, options)) {}

	public ActivityAwareOperation(IOperation<T> operation, IActivityReceiver subject, ActivityReceiverState state)
	{
		_operation = operation;
		_subject   = subject;
		_state     = state;
	}

	public async ValueTask Get(T parameter)
	{
		await _subject.On(_state);
		try
		{
			await _operation.On(parameter);
		}
		finally
		{
			await _subject.Off();
		}
	}
}