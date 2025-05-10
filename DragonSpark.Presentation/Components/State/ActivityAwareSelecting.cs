using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class ActivityAwareSelecting<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly IActivityReceiver     _subject;
	readonly ActivityReceiverState _state;

	public ActivityAwareSelecting(ISelecting<TIn, TOut> previous, IActivityReceiver subject)
		: this(previous, subject, ActivityOptions.Default) {}

	public ActivityAwareSelecting(ISelecting<TIn, TOut> previous, IActivityReceiver subject, ActivityOptions input)
		: this(previous, subject, new ActivityReceiverState(previous, input)) {}

	public ActivityAwareSelecting(ISelecting<TIn, TOut> previous, IActivityReceiver subject,
	                              ActivityReceiverState state)
	{
		_previous = previous;
		_subject  = subject;
		_state    = state;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var previous = _previous.Get(parameter);
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