using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	sealed class ActivityAwareOperation : IOperation
	{
		readonly IOperation      _operation;
		readonly object          _subject;
		readonly IUpdateActivity _activity;

		public ActivityAwareOperation(IOperation operation, object subject)
			: this(operation, subject, UpdateActivity.Default) {}

		public ActivityAwareOperation(IOperation operation, object subject, IUpdateActivity activity)
		{
			_operation = operation;
			_subject   = subject;
			_activity  = activity;
		}

		public async ValueTask Get()
		{
			_activity.Assign(_subject, _operation);
			try
			{
				await _operation.Await();
			}
			finally
			{
				_activity.Execute(_subject);
			}
		}
	}

	sealed class ActivityAwareOperation<T> : IOperation<T>
	{
		readonly IOperation<T>   _operation;
		readonly object          _subject;
		readonly IUpdateActivity _activity;

		public ActivityAwareOperation(IOperation<T> operation, object subject)
			: this(operation, subject, UpdateActivity.Default) {}

		public ActivityAwareOperation(IOperation<T> operation, object subject, IUpdateActivity activity)
		{
			_operation = operation;
			_subject   = subject;
			_activity  = activity;
		}

		public async ValueTask Get(T parameter)
		{
			_activity.Assign(_subject, _operation);
			try
			{
				await _operation.Await(parameter);
			}
			finally
			{
				_activity.Execute(_subject);
			}
		}
	}
}