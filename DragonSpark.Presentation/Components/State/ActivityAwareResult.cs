using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class ActivityAwareResult<T> : IResulting<T?>
{
	readonly IResulting<T?>        _previous;
	readonly IActivityReceiver     _subject;
	readonly ActivityReceiverState _state;

	public ActivityAwareResult(IResulting<T?> previous, IActivityReceiver subject, ActivityOptions options)
		: this(previous, subject, new ActivityReceiverState(previous, options)) {}

	public ActivityAwareResult(IResulting<T?> previous, IActivityReceiver subject, ActivityReceiverState state)
	{
		_previous = previous;
		_subject  = subject;
		_state    = state;
	}

	public async ValueTask<T?> Get()
	{
		var previous = _previous.Get();
		if (previous.IsCompleted)
		{
			return previous.Result;
		}

		await _subject.On(_state);
		try
		{
			return await previous.On();
		}
		finally
		{
			await _subject.Off();
		}
	}
}