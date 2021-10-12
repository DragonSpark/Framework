using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{
	public class ActivityAwareSelecting<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly ISelecting<TIn, TOut>   _previous;
		readonly object                  _subject;
		readonly IUpdateActivityReceiver _activity;

		public ActivityAwareSelecting(ISelecting<TIn, TOut> previous, object subject)
			: this(previous, subject, UpdateActivityReceiver.Default) {}

		public ActivityAwareSelecting(ISelecting<TIn, TOut> previous, object subject, IUpdateActivityReceiver activity)
		{
			_previous = previous;
			_subject  = subject;
			_activity = activity;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			await _activity.Get((_subject, _previous));
			try
			{
				return await _previous.Get(parameter);
			}
			finally
			{
				await _activity.Get(_subject);
			}
		}
	}
}