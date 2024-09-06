using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class ActivityAwareOperation : IOperation
{
	readonly IOperation              _operation;
	readonly object                  _subject;
	readonly ActivityReceiver        _input;
	readonly IUpdateActivityReceiver _update;

	public ActivityAwareOperation(IOperation operation, object subject)
		: this(operation, subject, ActivityOptions.Default) {}

	public ActivityAwareOperation(IOperation operation, object subject, ActivityOptions input)
		: this(operation, subject, new(operation, input), UpdateActivityReceiver.Default) {}

	// ReSharper disable once TooManyDependencies
	public ActivityAwareOperation(IOperation operation, object subject, ActivityReceiver input,
	                              IUpdateActivityReceiver update)
	{
		_operation = operation;
		_subject   = subject;
		_input     = input;
		_update    = update;
	}

	public async ValueTask Get()
	{
		var operation = _operation.Get();
		if (!operation.IsCompleted)
		{
			await _update.Get((_subject, _input));
			try
			{
				await operation;
			}
			finally
			{
				await _update.Await(_subject);
			}
		}
	}
}

sealed class ActivityAwareOperation<T> : IOperation<T>
{
	readonly IOperation<T>           _operation;
	readonly object                  _subject;
	readonly ActivityReceiver        _input;
	readonly IUpdateActivityReceiver _update;

	public ActivityAwareOperation(IOperation<T> operation, object subject)
		: this(operation, subject, ActivityOptions.Default) {}

	public ActivityAwareOperation(IOperation<T> operation, object subject, ActivityOptions options)
		: this(operation, subject, new(operation, options), UpdateActivityReceiver.Default) {}

	// ReSharper disable once TooManyDependencies
	public ActivityAwareOperation(IOperation<T> operation, object subject, ActivityReceiver input,
	                              IUpdateActivityReceiver update)
	{
		_operation = operation;
		_subject   = subject;
		_input     = input;
		_update    = update;
	}

	public async ValueTask Get(T parameter)
	{
		var operation = _operation.Get(parameter);
		if (!operation.IsCompleted)
		{
			await _update.Get((_subject, _input));
			try
			{
				await operation;
			}
			finally
			{
				await _update.Await(_subject);
			}
		}
	}
}