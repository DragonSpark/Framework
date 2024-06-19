using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class ActivityAwareOperation : IOperation
{
	readonly IOperation              _operation;
	readonly object                  _subject;
	readonly ActivityReaderInstance  _input;
	readonly IUpdateActivityReceiver _update;

	public ActivityAwareOperation(IOperation operation, object subject)
		: this(operation, subject, ActivityReceiverInput.Default) {}

	public ActivityAwareOperation(IOperation operation, object subject, ActivityReceiverInput input)
		: this(operation, subject, new(operation, input), UpdateActivityReceiver.Default) {}

	// ReSharper disable once TooManyDependencies
	public ActivityAwareOperation(IOperation operation, object subject, ActivityReaderInstance input,
	                              IUpdateActivityReceiver update)
	{
		_operation  = operation;
		_subject    = subject;
		_input = input;
		_update     = update;
	}

	public async ValueTask Get()
	{
		await _update.Get(Pairs.Create(_subject, _input));
		try
		{
			await _operation.Get();
		}
		finally
		{
			await _update.Await(_subject);
		}
	}
}

sealed class ActivityAwareOperation<T> : IOperation<T>
{
	readonly IOperation<T>           _operation;
	readonly object                  _subject;
	readonly ActivityReaderInstance  _input;
	readonly IUpdateActivityReceiver _update;

	public ActivityAwareOperation(IOperation<T> operation, object subject)
		: this(operation, subject, ActivityReceiverInput.Default) {}

	public ActivityAwareOperation(IOperation<T> operation, object subject, ActivityReceiverInput input)
		: this(operation, subject, new(operation, input), UpdateActivityReceiver.Default) {}

	// ReSharper disable once TooManyDependencies
	public ActivityAwareOperation(IOperation<T> operation, object subject, ActivityReaderInstance input,
	                              IUpdateActivityReceiver update)
	{
		_operation  = operation;
		_subject    = subject;
		_input = input;
		_update     = update;
	}

	public async ValueTask Get(T parameter)
	{
		await _update.Get(Pairs.Create(_subject, _input));
		try
		{
			await _operation.Get(parameter);
		}
		finally
		{
			await _update.Await(_subject);
		}
	}
}