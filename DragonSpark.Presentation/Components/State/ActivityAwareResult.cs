using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class ActivityAwareResult<T> : IResulting<T?>
{
	readonly IResulting<T?>          _previous;
	readonly object                  _subject;
	readonly ActivityReceiver  _input;
	readonly IUpdateActivityReceiver _activity;

	/*public ActivityAwareResult(IResulting<T?> previous, object subject)
		: this(previous, subject, ActivityReceiverInput.Default) {}*/

	public ActivityAwareResult(IResulting<T?> previous, object subject, ActivityReceiverInput input)
		: this(previous, subject, new(previous, input), UpdateActivityReceiverAlternate.Default) {}

	// ReSharper disable once TooManyDependencies
	public ActivityAwareResult(IResulting<T?> previous, object subject, ActivityReceiver input,
	                           IUpdateActivityReceiver activity)
	{
		_previous = previous;
		_subject  = subject;
		_input    = input;
		_activity = activity;
	}

	public async ValueTask<T?> Get()
	{
		await _activity.Get((_subject, _input));
		try
		{
			return await _previous.Get();
		}
		finally
		{
			await _activity.Await(_subject);
		}
	}
}