using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{


	sealed class ActivityAwareOperation : IOperation
	{
		readonly IOperation              _operation;
		readonly object                  _subject;
		readonly IUpdateActivityReceiver _activity;

		public ActivityAwareOperation(IOperation operation, object subject)
			: this(operation, subject, UpdateActivityReceiver.Default) {}

		public ActivityAwareOperation(IOperation operation, object subject, IUpdateActivityReceiver activity)
		{
			_operation = operation;
			_subject   = subject;
			_activity  = activity;
		}

		public async ValueTask Get()
		{
			await _activity.Get((_subject, _operation));
			try
			{
				await _operation.Get();
			}
			finally
			{
				await _activity.Get(_subject);
			}
		}
	}

	sealed class ActivityAwareOperation<T> : IOperation<T>
	{
		readonly IOperation<T>           _operation;
		readonly object                  _subject;
		readonly IUpdateActivityReceiver _activity;

		public ActivityAwareOperation(IOperation<T> operation, object subject)
			: this(operation, subject, UpdateActivityReceiver.Default) {}

		public ActivityAwareOperation(IOperation<T> operation, object subject, IUpdateActivityReceiver activity)
		{
			_operation = operation;
			_subject   = subject;
			_activity  = activity;
		}

		public async ValueTask Get(T parameter)
		{
			await _activity.Get((_subject, _operation));
			try
			{
				await _operation.Get(parameter);
			}
			finally
			{
				await _activity.Get(_subject);
			}
		}
	}

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
			_subject       = subject;
			_activity      = activity;
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