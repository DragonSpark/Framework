using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{
	sealed class ActivityAwareResult<T> : IResulting<T?>
	{
		readonly IResulting<T?>          _previous;
		readonly object                  _subject;
		readonly IUpdateActivityReceiver _activity;

		public ActivityAwareResult(IResulting<T?> previous, object subject)
			: this(previous, subject, UpdateActivityReceiver.Default) {}

		public ActivityAwareResult(IResulting<T?> previous, object subject, IUpdateActivityReceiver activity)
		{
			_previous = previous;
			_subject  = subject;
			_activity = activity;
		}

		public async ValueTask<T?> Get()
		{
			await _activity.Await(Pairs.Create<object, object>(_subject, _previous));
			try
			{
				return await _previous.Await();
			}
			finally
			{
				await _activity.Await(_subject);
			}
		}
	}
}