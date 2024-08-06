using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class ActivityAwareSelecting<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut>   _previous;
	readonly object                  _subject;
	readonly ActivityReceiver  _input;
	readonly IUpdateActivityReceiver _update;

	public ActivityAwareSelecting(ISelecting<TIn, TOut> previous, object subject)
		: this(previous, subject, ActivityReceiverInput.Default) {}

	public ActivityAwareSelecting(ISelecting<TIn, TOut> previous, object subject, ActivityReceiverInput input)
		: this(previous, subject, new(previous, input), UpdateActivityReceiver.Default) {}

	// ReSharper disable once TooManyDependencies
	public ActivityAwareSelecting(ISelecting<TIn, TOut> previous, object subject, ActivityReceiver input,
	                              IUpdateActivityReceiver update)
	{
		_previous = previous;
		_subject  = subject;
		_input    = input;
		_update   = update;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		await _update.Get(Pairs.Create(_subject, _input));
		try
		{
			return await _previous.Get(parameter);
		}
		finally
		{
			await _update.Await(_subject);
		}
	}
}