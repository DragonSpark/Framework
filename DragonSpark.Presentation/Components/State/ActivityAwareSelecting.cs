using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class ActivityAwareSelecting<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut>   _previous;
	readonly object                  _subject;
	readonly ActivityReceiver        _input;
	readonly IUpdateActivityReceiver _update;

	public ActivityAwareSelecting(ISelecting<TIn, TOut> previous, object subject)
		: this(previous, subject, UpdateActivityReceiver.Default) {}

	public ActivityAwareSelecting(ISelecting<TIn, TOut> previous, object subject, IUpdateActivityReceiver update)
		: this(previous, subject, ActivityOptions.Default, update) {}

	// ReSharper disable once TooManyDependencies
	public ActivityAwareSelecting(ISelecting<TIn, TOut> previous, object subject, ActivityOptions input,
	                              IUpdateActivityReceiver update)
		: this(previous, subject, new ActivityReceiver(previous, input), update) {}

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
		var previous = _previous.Get(parameter);
		if (previous.IsCompleted)
		{
			return previous.Result;
		}

		await _update.Get(Pairs.Create(_subject, _input)).On();
		try
		{
			return await previous.On();
		}
		finally
		{
			await _update.Off(_subject);
		}
	}
}