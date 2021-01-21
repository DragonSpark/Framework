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
}